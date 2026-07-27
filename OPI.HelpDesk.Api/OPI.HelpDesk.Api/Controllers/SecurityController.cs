using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPI.HelpDesk.Application.Dtos.Security;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Interfaces.Security;

namespace OPI.HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController(ISecurityService securityService) : ControllerBase
    {
        [HttpPost("SingUp")]
        public async Task<ResponseWrapper<AuthResponseDto>> SingUp([FromBody] SingUpRequestDto request, CancellationToken ct)
        {
            try
            {
                AuthResponseDto result = await securityService.SingUp(request, ct);
                return new ResponseWrapper<AuthResponseDto>(
                   StatusCodes.Status201Created,
                   result,
                   true
                );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<AuthResponseDto>(
                    StatusCodes.Status400BadRequest,
                    null,
                    false,
                    ex.Message
                );
            }
        }

        [HttpPost("Login")]
        public async Task<ResponseWrapper<AuthResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
        {
            try
            {
                AuthResponseDto result = await securityService.Login(request, ct);
                return new ResponseWrapper<AuthResponseDto>(
                    StatusCodes.Status200OK,
                    result,
                    true
                 );
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<AuthResponseDto>(
                    StatusCodes.Status400BadRequest,
                    null,
                    false,
                    ex.Message
                );
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new
            {
                message = "Sesión cerrada correctamente."
            });
        }
    }
}
