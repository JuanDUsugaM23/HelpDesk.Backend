using OPI.HelpDesk.Application.Dtos.Tickets;
using OPI.HelpDesk.Application.DTOs.Ticket;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using OPI.HelpDesk.Application.Interfaces.Ticket;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Mappings;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDesk.Application.Specifications.Tickets;
using OPI.HelpDesk.Application.Specifications.Users;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public TicketService(IUnitOfWork unitOfWork,ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<TicketResponse> CreateAsync(CreateTicketRequest request)
        {
            if (request.ClientId == Guid.Empty)
                throw new ArgumentException("El cliente es obligatorio.");

            var clientSpecification = new GetTicketClientSpecification(request.ClientId);
            var client = await _unitOfWork.Repository<User>().GetAsync(clientSpecification);
            if (client is null || !client.Enable)
                throw new InvalidOperationException("El cliente no existe o está desactivado.");
            if (!Enum.TryParse<TicketCategoriesEnum>(request.Category, true, out var category))
            {
                throw new ArgumentException("La categoría enviada no existe.");
            }

            if (!Enum.TryParse<TicketPriorityEnum>(request.Priority, true, out var priority))
            {
                throw new ArgumentException("La prioridad enviada no existe.");
            }

            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Category = category,
                Priority = priority,

                Status = TicketStatusEnum.Open,

                Client = client,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.ClientId,

                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = request.ClientId,

                Enable = true,

                AssignedTechnicalId = null,

                SlaLimitTime = DateTime.UtcNow.AddHours(24)
            };

            await _unitOfWork.Repository<Ticket>().AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return TicketMapper.ToResponse(ticket);

        }

        public async Task<IEnumerable<TicketResponse>> GetAllAsync(GetAllTicketsQueryDto query)
        {
            var userId = _currentUser.UserId;

            var user = await _unitOfWork
            .Repository<User>()
            .GetAsync(new UserByIdSpecification(userId));

            if (user is null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            var specification = new GetAllTicketsSpecification(query, user);

            var tickets = await _unitOfWork
                .Repository<Ticket>()
                .GetAllAsync(specification);

            return tickets.Select(TicketMapper.ToResponse);
        }

        public async Task<TicketResponse?> GetByIdAsync(Guid id)
        {
            var specification = new GetTicketByIdSpecification(id);

            var ticket = await _unitOfWork
                .Repository<Ticket>()
                .GetAsync(specification);

            if (ticket is null)
                return null;

            EnsureCanAccessTicket(ticket);

            return TicketMapper.ToResponse(ticket);
        }

        public async Task<TicketResponse?> UpdateAsync(Guid id, UpdateTicketRequest request)
        {
            var specification = new GetTicketByIdSpecification(id);

            var ticket = await _unitOfWork
                .Repository<Ticket>()
                .GetAsync(specification);

            if (ticket is null)
                return null;

            ticket.Title = request.Title;
            ticket.Description = request.Description;
            ticket.Category = request.Category;
            ticket.Priority = request.Priority;

            ticket.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Ticket>().Update(ticket);

            await _unitOfWork.SaveChangesAsync();

            return TicketMapper.ToResponse(ticket);
        }

        public async Task<TicketResponse?> AssignAsync(Guid id, AssignTicketRequest request)
        {
            if (request.TechnicalId == Guid.Empty)
                throw new ArgumentException("El técnico es obligatorio.");

            var technicianSpecification = new GetTicketTechnicianSpecification(request.TechnicalId);
            var technician = await _unitOfWork.Repository<User>().GetAsync(technicianSpecification);
            if (technician is null)
                throw new InvalidOperationException("El técnico no existe, está desactivado o no tiene el rol requerido.");

            var ticketSpecification = new GetTicketByIdSpecification(id);
            var ticket = await _unitOfWork.Repository<Ticket>().GetAsync(ticketSpecification);
            if (ticket is null)
                return null;

            if (!technician.Specialities.Contains(ticket.Category))
                throw new InvalidOperationException(
                    $"El técnico no está habilitado para atender tickets de la categoría {ticket.Category}.");

            if (ticket.Status is not TicketStatusEnum.Open and not TicketStatusEnum.Reopened)
                throw new InvalidOperationException("Solo se pueden asignar tickets abiertos o reabiertos.");

            var activeTickets = await _unitOfWork.Repository<Ticket>().GetAllAsync(
                new SpecificationBuilder<Ticket>().Where(activeTicket =>
                    activeTicket.Enable &&
                    activeTicket.AssignedTechnicalId == technician.Id &&
                    (activeTicket.Status == TicketStatusEnum.Assigned ||
                     activeTicket.Status == TicketStatusEnum.InProgress ||
                     activeTicket.Status == TicketStatusEnum.Reopened)));

            if (activeTickets.Count() >= technician.MaxTicket)
                throw new InvalidOperationException(
                    $"El técnico ya alcanzó su límite de {technician.MaxTicket} tickets activos.");

            var previousStatus = ticket.Status;
            ticket.AssignedTechnicalId = technician.Id;
            ticket.Status = TicketStatusEnum.Assigned;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.UpdatedBy = _currentUser.UserId;
            await AddHistoryAsync(ticket, previousStatus, TicketStatusEnum.Assigned,
                "Ticket asignado a un técnico.");
            _unitOfWork.Repository<Ticket>().Update(ticket);
            await _unitOfWork.SaveChangesAsync();

            return TicketMapper.ToResponse(ticket);
        }

        public async Task<TicketResponse?> ChangeStatusAsync(Guid id, ChangeTicketStatusRequest request)
        {
            var ticket = await _unitOfWork.Repository<Ticket>()
                .GetAsync(new GetTicketByIdSpecification(id));

            if (ticket is null)
                return null;

            var currentUser = await _unitOfWork.Repository<User>()
                .GetAsync(new UserByIdSpecification(_currentUser.UserId));
            if (currentUser is null)
                throw new UnauthorizedAccessException("Usuario autenticado no encontrado.");

            ValidateTransition(ticket, request.Status, currentUser);

            if (request.Status == TicketStatusEnum.Resolved && string.IsNullOrWhiteSpace(request.Note))
                throw new InvalidOperationException("Para resolver el ticket debes registrar un comentario de resolución.");

            if (request.Status == TicketStatusEnum.Closed)
            {
                var resolutionComment = await _unitOfWork.Repository<Comment>().GetAsync(
                    new SpecificationBuilder<Comment>().Where(c =>
                        c.TicketId == ticket.Id &&
                        c.CreatedBy == ticket.AssignedTechnicalId &&
                        c.Enable &&
                        !string.IsNullOrWhiteSpace(c.Text)));

                if (resolutionComment is null)
                    throw new InvalidOperationException(
                        "No se puede cerrar el ticket sin un comentario de resolución del técnico asignado.");
            }

            var previousStatus = ticket.Status;
            ticket.Status = request.Status;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.UpdatedBy = currentUser.Id;

            if (request.Status == TicketStatusEnum.Resolved)
            {
                ticket.ResolvedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Comment>().AddAsync(new Comment
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticket.Id,
                    Text = request.Note,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = currentUser.Id,
                    Enable = true
                });
            }
            if (request.Status == TicketStatusEnum.Closed)
                ticket.ClosedAt = DateTime.UtcNow;

            await AddHistoryAsync(ticket, previousStatus, request.Status, request.Note);
            _unitOfWork.Repository<Ticket>().Update(ticket);
            await _unitOfWork.SaveChangesAsync();

            return TicketMapper.ToResponse(ticket);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var specification = new GetTicketByIdSpecification(id);

            var ticket = await _unitOfWork
                .Repository<Ticket>()
                .GetAsync(specification);

            if (ticket is null)
                return false;

            _unitOfWork.Repository<Ticket>().Delete(ticket);

            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        private async Task AddHistoryAsync(Ticket ticket, TicketStatusEnum from, TicketStatusEnum to, string? note)
        {
            await _unitOfWork.Repository<TicketHistoryLog>().AddAsync(new TicketHistoryLog
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                FromStatus = from.ToString(),
                ToStatus = to.ToString(),
                Note = note,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = _currentUser.UserId,
                Enable = true
            });
        }

        private static void ValidateTransition(Ticket ticket, TicketStatusEnum targetStatus, User currentUser)
        {
            var isSupervisor = currentUser.Role == RolesEnum.Supervisor;
            var isAssignedTechnician = currentUser.Role == RolesEnum.Technical &&
                ticket.AssignedTechnicalId == currentUser.Id;
            var isClientOwner = currentUser.Role == RolesEnum.Client && ticket.CreatedBy == currentUser.Id;

            var valid = (ticket.Status, targetStatus) switch
            {
                (TicketStatusEnum.Assigned, TicketStatusEnum.InProgress) => isAssignedTechnician || isSupervisor,
                (TicketStatusEnum.InProgress, TicketStatusEnum.Resolved) => isAssignedTechnician || isSupervisor,
                (TicketStatusEnum.Resolved, TicketStatusEnum.Closed) => isClientOwner || isSupervisor,
                (TicketStatusEnum.Resolved, TicketStatusEnum.Reopened) =>
                    isClientOwner && ticket.ResolvedAt.HasValue && ticket.ResolvedAt.Value.AddDays(3) >= DateTime.UtcNow,
                (TicketStatusEnum.Reopened, TicketStatusEnum.Assigned) => isSupervisor,
                _ => false
            };

            if (!valid)
                throw new InvalidOperationException(
                    $"No se permite cambiar el ticket de {ticket.Status} a {targetStatus} con el rol actual.");
        }

        private void EnsureCanAccessTicket(Ticket ticket)
        {
            if (_currentUser.Role == RolesEnum.Supervisor.ToString())
                return;

            if (_currentUser.Role == RolesEnum.Client.ToString() && ticket.CreatedBy == _currentUser.UserId)
                return;

            if (_currentUser.Role == RolesEnum.Technical.ToString() && ticket.AssignedTechnicalId == _currentUser.UserId)
                return;

            throw new UnauthorizedAccessException("No tienes permiso para acceder a este ticket.");
        }
    }
}
