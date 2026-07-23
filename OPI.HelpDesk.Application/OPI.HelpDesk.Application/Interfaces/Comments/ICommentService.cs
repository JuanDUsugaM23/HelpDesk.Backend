using OPI.HelpDesk.Application.Dtos.Comments;
using OPI.HelpDesk.Application.Dtos.Shares;

namespace OPI.HelpDesk.Application.Interfaces.Comments
{
    public interface ICommentService
    {
        Task<CommentResponseDto> AddCommnet(CommentAddRequestDto request, CancellationToken ct);
        Task<GetAllResponseDto<CommentResponseDto>> GetAllCommnets(GetAllCommentsQueryDto query, CancellationToken ct);
        Task<CommentResponseDto> GetByIdCommnet(Guid requidest, CancellationToken ct);
        Task<CommentResponseDto> UpdateCommnet(CommentEditRequestDto request, CancellationToken ct);
    }
}
