using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OPI.HelpDesk.Application.Dtos.Comments;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Interfaces.Comments;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComentController(ICommentService commentService) : ControllerBase
    {
        [HttpPost("Add")]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technican)}, {nameof(RolesEnum.Client)}")]
        public async Task<ResponseWrapper<CommentResponseDto>> Add([FromBody]CommentAddRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await commentService.AddCommnet(request, ct);
                return new ResponseWrapper<CommentResponseDto>(
                        StatusCodes.Status201Created,
                        result,
                        true
                );
            }
            catch (Exception ex) 
            {
                return new ResponseWrapper<CommentResponseDto>(
                       StatusCodes.Status400BadRequest,
                       null,
                       false,
                       ex.Message
               );
            }
        }

        [HttpGet()]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technican)}, {nameof(RolesEnum.Client)}")]
        public async Task<ResponseWrapper<GetAllResponseDto<CommentResponseDto>>> GetAll([FromQuery]GetAllCommentsQueryDto query, CancellationToken ct)
        {
            try
            {
                var result = await commentService.GetAllCommnets(query, ct);
                return new ResponseWrapper<GetAllResponseDto<CommentResponseDto>>(
                        StatusCodes.Status201Created,
                        result,
                        true
                );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<GetAllResponseDto<CommentResponseDto>>(
                       StatusCodes.Status500InternalServerError,
                       null,
                       false,
                       ex.Message
               );
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technican)}, {nameof(RolesEnum.Client)}")]
        public async Task<ResponseWrapper<CommentResponseDto>> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await commentService.GetByIdCommnet(id, ct);
                return new ResponseWrapper<CommentResponseDto>(
                        StatusCodes.Status201Created,
                        result,
                        true
                );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<CommentResponseDto>(
                       StatusCodes.Status500InternalServerError,
                       null,
                       false,
                       ex.Message
               );
            }
        }

        [HttpPut("Update")]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technican)}, {nameof(RolesEnum.Client)}")]
        public async Task<ResponseWrapper<CommentResponseDto>> Update(CommentEditRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await commentService.UpdateCommnet(request, ct);
                return new ResponseWrapper<CommentResponseDto>(
                        StatusCodes.Status201Created,
                        result,
                        true
                );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<CommentResponseDto>(
                       StatusCodes.Status400BadRequest,
                       null,
                       false,
                       ex.Message
               );
            }
        }
    }
}
