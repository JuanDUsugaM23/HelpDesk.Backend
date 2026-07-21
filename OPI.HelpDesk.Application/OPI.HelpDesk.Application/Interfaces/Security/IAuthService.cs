using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Interfaces.Security
{
    public interface IAuthService
    {
        string GenerateJwtToken(User userEntity);
        RefreshToken GenerateRefreshToken(Guid userId);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
