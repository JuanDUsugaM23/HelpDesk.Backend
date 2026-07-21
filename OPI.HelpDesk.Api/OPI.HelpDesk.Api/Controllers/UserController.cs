using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Dtos.Users;
using OPI.HelpDesk.Application.Interfaces.Users;

namespace OPI.HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<ResponseWrapper<UserResponseDto>> Add([FromBody] UserAddRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await userService.AddUser(request, ct);
                return new ResponseWrapper<UserResponseDto>(
                        StatusCodes.Status201Created,
                        result,
                        true
                    );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<UserResponseDto>(
                        StatusCodes.Status500InternalServerError,
                        null,
                        false,
                        ex.Message
                );
            }

        }

        [HttpGet("GetAll")]
        public async Task<ResponseWrapper<GetAllResponseDto<UserResponseDto>>> GetAll([FromQuery] GetAllUserQueryDto query, CancellationToken ct)
        {
            try
            {
                var result = await userService.GetAllUsers(query, ct);
                return new ResponseWrapper<GetAllResponseDto<UserResponseDto>>(
                        StatusCodes.Status200OK,
                        result,
                        true
                    );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<GetAllResponseDto<UserResponseDto>>(
                    StatusCodes.Status404NotFound,
                    null,
                    false,
                    ex.Message
                    );
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ResponseWrapper<UserResponseDto>> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await userService.GetByIdUser(id, ct);
                return new ResponseWrapper<UserResponseDto>(
                    StatusCodes.Status200OK,
                    result,
                    true
                    );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<UserResponseDto>(
                    StatusCodes.Status404NotFound,
                    null,
                    false,
                    ex.Message
                    );
            }
        }

        [HttpPut]
        public async Task<ResponseWrapper<UserResponseDto>> Update([FromBody] UserEditRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await userService.UpdateUser(request, ct);
                return new ResponseWrapper<UserResponseDto>(
                    StatusCodes.Status200OK,
                    result,
                    true
                    );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<UserResponseDto>(
                    StatusCodes.Status500InternalServerError,
                    null,
                    false,
                    ex.Message
                    );
            }
        }
    }
}
