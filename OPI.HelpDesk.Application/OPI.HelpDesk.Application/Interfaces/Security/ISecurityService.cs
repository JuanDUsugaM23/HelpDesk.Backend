using OPI.HelpDesk.Application.Dtos.Security;

namespace OPI.HelpDesk.Application.Interfaces.Security
{
    public interface ISecurityService
    {
        Task<AuthResponseDto> SingUp(SingUpRequestDto request, CancellationToken ct);
        Task<AuthResponseDto> Login(LoginRequestDto request, CancellationToken ct);
        void Logout();
    }
}
