using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Dtos.Users;

namespace OPI.HelpDesk.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserResponseDto> AddUser(UserAddRequestDto request, CancellationToken ct);
        Task<GetAllResponseDto<UserResponseDto>> GetAllUsers(GetAllUserQueryDto query, CancellationToken ct);
        Task<UserResponseDto> GetByIdUser(Guid id, CancellationToken ct);
        Task<UserResponseDto> UpdateUser(UserEditRequestDto request, CancellationToken ct);

    }
}
