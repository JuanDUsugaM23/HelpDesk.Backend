using OPI.HelpDesk.Application.Dtos.Comments;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Interfaces.Comments;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDesk.Application.Specifications.Comments;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Services.Comments
{
    public class CommentService(IUnitOfWork uow, ICurrentUserService currentUser) : ICommentService
    {
        public async Task<CommentResponseDto> AddCommnet(CommentAddRequestDto request, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<Ticket>()
                .Where(t => t.Id == request.TicketId);
            var ticket = await uow.Repository<Ticket>().GetAsync(spec, ct);

            if(ticket == null || ticket.Enable != true)
            {
                throw new Exception("Ticket no registardo en base de datos o esta inhabilitado.");
            }
            EnsureCanAccessTicket(ticket);

            var newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                TicketId = request.TicketId
            };
            if(request.Text is not null) newComment.Text = request.Text;
            
            await uow.Repository<Comment>().AddAsync(newComment);
            await uow.SaveChangesAsync();
            uow.Dispose();

            return ToResponse(newComment);
        }

        public async Task<GetAllResponseDto<CommentResponseDto>> GetAllCommnets(GetAllCommentsQueryDto query, CancellationToken ct)
        {
            var spec = new CommentSpecificationFilter(query);
            ApplyAccessFilter(spec);
            var comments = await uow.Repository<Comment>().GetAllAsync(spec, ct);

            return new GetAllResponseDto<CommentResponseDto>()
            {
                Data = comments.Select(ToResponse),
                Count = comments.Count(),
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<CommentResponseDto> GetByIdCommnet(Guid id, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<Comment>()
                .Where( c => c.Id == id && c.Enable == true)
                .Include(c => c.Ticket);

            var comment = await uow.Repository<Comment>().GetAsync(spec, ct);

            if (comment is null)
                throw new KeyNotFoundException("Comentario no encontrado.");

            EnsureCanAccessTicket(comment.Ticket);

            return ToResponse(comment);
        }

        public async Task<CommentResponseDto> UpdateCommnet(CommentEditRequestDto request, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<Ticket>()
                .Where(t => t.Id == request.TicketId);
            var ticket = await uow.Repository<Ticket>().GetAsync(spec, ct);

            if (ticket == null || ticket.Enable != true)
            {
                throw new Exception("Ticket no está registardo en base de datos o esta inhabilitado.");
            }
            EnsureCanAccessTicket(ticket);

            var specComment = new SpecificationBuilder<Comment>()
               .Where(c => c.Id == request.Id);
            var commentUpdated = await uow.Repository<Comment>().GetAsync(specComment, ct);
            if (commentUpdated == null || commentUpdated.Enable != true)
            {
                throw new Exception("El comentario no esta registardo en base de datos o esta inhabilitado.");
            }
            if (currentUser.Role != RolesEnum.Supervisor.ToString() && commentUpdated.CreatedBy != currentUser.UserId)
                throw new UnauthorizedAccessException("Solo el autor o un supervisor puede editar el comentario.");
            if (request.Text is not null) commentUpdated.Text = request.Text;
            if(request.TicketId is not null) commentUpdated.TicketId = request.TicketId.Value;
            if (request.Enable is not null) commentUpdated.Enable = request.Enable.Value; 

            uow.Repository<Comment>().Update(commentUpdated);
            await uow.SaveChangesAsync();
            uow.Dispose();

            return ToResponse(commentUpdated);
        }

        private static CommentResponseDto ToResponse(Comment comment) => new()
        {
            Id = comment.Id,
            TicketId = comment.Ticket.Id,
            TickedTitle = comment.Ticket.Title,
            Text = comment.Text,
            Enable = comment.Enable,
        };

        private void ApplyAccessFilter(CommentSpecificationFilter spec)
        {
            if (currentUser.Role == RolesEnum.Supervisor.ToString())
                return;

            if (currentUser.Role == RolesEnum.Client.ToString())
            {
                spec.AddAccessCriteria(c => c.Ticket.CreatedBy == currentUser.UserId);
                return;
            }

            if (currentUser.Role == RolesEnum.Technical.ToString())
            {
                spec.AddAccessCriteria(c => c.Ticket.AssignedTechnicalId == currentUser.UserId);
                return;
            }

            throw new UnauthorizedAccessException("Rol no autorizado para consultar comentarios.");
        }

        private void EnsureCanAccessTicket(Ticket ticket)
        {
            if (currentUser.Role == RolesEnum.Supervisor.ToString() ||
                (currentUser.Role == RolesEnum.Client.ToString() && ticket.CreatedBy == currentUser.UserId) ||
                (currentUser.Role == RolesEnum.Technical.ToString() && ticket.AssignedTechnicalId == currentUser.UserId))
                return;

            throw new UnauthorizedAccessException("No tienes permiso para acceder a los comentarios de este ticket.");
        }
    }
}
