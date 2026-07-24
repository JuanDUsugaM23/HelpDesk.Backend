using OPI.HelpDesk.Application.Dtos.Tickets;
using OPI.HelpDesk.Application.DTOs.Ticket;
using OPI.HelpDesk.Application.Interfaces.Ticket;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Mappings;
using OPI.HelpDesk.Application.Specifications.Tickets;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var specification = new GetAllTicketsSpecification(query);

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

            ticket.AssignedTechnicalId = technician.Id;
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
    }
}
