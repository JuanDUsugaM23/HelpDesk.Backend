namespace OPI.HelpDesk.Application.Dtos.Security
{
    public record AuthResponseDto
    (
        string AccesToken,
        string RefreshToken,
        DateTime Expiration
    );
}
