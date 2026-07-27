using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technical)}, {nameof(RolesEnum.Client)}")]
        public async Task<IActionResult> Add([FromBody] CommentAddRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await commentService.AddCommnet(request, ct);
                return StatusCode(201, new ResponseWrapper<CommentResponseDto>(201, result, true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ResponseWrapper<CommentResponseDto>(403, null, false, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseWrapper<CommentResponseDto>(400, null, false, ex.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technical)}, {nameof(RolesEnum.Client)}")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCommentsQueryDto query, CancellationToken ct)
        {
            try
            {
                var result = await commentService.GetAllCommnets(query, ct);
                return Ok(new ResponseWrapper<GetAllResponseDto<CommentResponseDto>>(200, result, true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ResponseWrapper<GetAllResponseDto<CommentResponseDto>>(403, null, false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<GetAllResponseDto<CommentResponseDto>>(500, null, false, ex.Message));
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technical)}, {nameof(RolesEnum.Client)}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await commentService.GetByIdCommnet(id, ct);
                return Ok(new ResponseWrapper<CommentResponseDto>(200, result, true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ResponseWrapper<CommentResponseDto>(403, null, false, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseWrapper<CommentResponseDto>(404, null, false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper<CommentResponseDto>(500, null, false, ex.Message));
            }
        }

        [HttpPut("Update")]
        [Authorize(Roles = $"{nameof(RolesEnum.Supervisor)}, {nameof(RolesEnum.Technical)}, {nameof(RolesEnum.Client)}")]
        public async Task<IActionResult> Update([FromBody] CommentEditRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await commentService.UpdateCommnet(request, ct);
                return Ok(new ResponseWrapper<CommentResponseDto>(200, result, true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ResponseWrapper<CommentResponseDto>(403, null, false, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseWrapper<CommentResponseDto>(400, null, false, ex.Message));
            }
        }
    }
}
