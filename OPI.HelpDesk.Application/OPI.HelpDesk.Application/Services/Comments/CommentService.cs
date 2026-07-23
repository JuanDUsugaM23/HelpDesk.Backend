using OPI.HelpDesk.Application.Dtos.Comments;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Interfaces.Comments;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDesk.Application.Specifications.Comments;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Services.Comments
{
    public class CommentService(IUnitOfWork uow) : ICommentService
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
            var comments = await uow.Repository<Comment>().GetAllAsync(spec, ct);

            return new GetAllResponseDto<CommentResponseDto>()
            {
                Data = comments.Select(ToResponse),
                Count = await uow.Repository<Comment>().CountAsync(),
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

            var specComment = new SpecificationBuilder<Comment>()
               .Where(c => c.Id == request.Id);
            var commentUpdated = await uow.Repository<Comment>().GetAsync(specComment, ct);
            if (commentUpdated == null || commentUpdated.Enable != true)
            {
                throw new Exception("El comentario no esta registardo en base de datos o esta inhabilitado.");
            }
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
    }
}
