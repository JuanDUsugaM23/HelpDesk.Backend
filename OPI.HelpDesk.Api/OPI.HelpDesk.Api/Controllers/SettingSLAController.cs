using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPI.HelpDesk.Application.Dtos.SettingSLAs;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Interfaces.SettingSLAs;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class SettingSLAController(ISettingSLA settingSLAService) : ControllerBase
    {
        [HttpPost("Add")]
        [Authorize(Roles = nameof(RolesEnum.Supervisor))]
        public async Task<ResponseWrapper<SettingSLAResponseDto>> Add([FromBody] SettingSLAAddRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await settingSLAService.AddSettingSla(request, ct);
                return new ResponseWrapper<SettingSLAResponseDto>(
                    StatusCodes.Status201Created,
                    result,
                    true
                    );
            }
            catch (Exception ex)
            {

                return new ResponseWrapper<SettingSLAResponseDto>(
                    StatusCodes.Status500InternalServerError,
                    null,
                    false,
                    ex.Message
                    );
            }
        }

        [HttpGet]
        [Authorize(Roles = nameof(RolesEnum.Supervisor))]
        public async Task<ResponseWrapper<GetAllResponseDto<SettingSLAResponseDto>>> GetAll([FromQuery] GetSettingSLAsQueryDto query, CancellationToken ct)
        {
            try
            {
                var result = await settingSLAService.GetAllSettingSLAs(query, ct);
                return new ResponseWrapper<GetAllResponseDto<SettingSLAResponseDto>>(
                    StatusCodes.Status201Created,
                    result,
                    true
                    );
            }
            catch (Exception ex)
            {

                return new ResponseWrapper<GetAllResponseDto<SettingSLAResponseDto>>(
                    StatusCodes.Status500InternalServerError,
                    null,
                    false,
                    ex.Message
                    );
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = nameof(RolesEnum.Supervisor))]
        public async Task<ResponseWrapper<SettingSLAResponseDto>> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await settingSLAService.GetByIdSettingSLA(id, ct);
                return new ResponseWrapper<SettingSLAResponseDto>(
                    StatusCodes.Status201Created,
                    result,
                    true
                    );
            }
            catch (Exception ex)
            {

                return new ResponseWrapper<SettingSLAResponseDto>(
                    StatusCodes.Status500InternalServerError,
                    null,
                    false,
                    ex.Message
                    );
            }
        }

        [HttpPut("Update")]
        [Authorize(Roles = nameof(RolesEnum.Supervisor))]
        public async Task<ResponseWrapper<SettingSLAResponseDto>> Update([FromBody] SettingSLAEditRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await settingSLAService.UpdateSettingSLA(request, ct);
                return new ResponseWrapper<SettingSLAResponseDto>(
                    StatusCodes.Status201Created,
                    result,
                    true
                    );
            }
            catch (Exception ex)
            {

                return new ResponseWrapper<SettingSLAResponseDto>(
                    StatusCodes.Status500InternalServerError,
                    null,
                    false,
                    ex.Message
                    );
            }
        }
    }
}
