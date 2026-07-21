using OPI.HelpDesk.Application.Dtos.SettingSLAs;
using OPI.HelpDesk.Application.Dtos.Shares;

namespace OPI.HelpDesk.Application.Interfaces.SettingSLAs
{
    public interface ISettingSLA
    {
        Task<SettingSLAResponseDto> AddSettingSla(SettingSLAAddRequestDto request, CancellationToken ct);
        Task<GetAllResponseDto<SettingSLAResponseDto>> GetAllSettingSLAs(GetSettingSLAsQueryDto query, CancellationToken ct);
        Task<SettingSLAResponseDto> GetByIdSettingSLA(Guid id, CancellationToken ct);
        Task<SettingSLAResponseDto> UpdateSettingSLA(SettingSLAEditRequestDto request, CancellationToken ct);
    }
}
