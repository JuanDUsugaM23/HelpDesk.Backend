using Microsoft.AspNetCore.Http;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using System.Security.Claims;

namespace OPI.HelpDesk.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public Guid UserId
        {
            get
            {
                var id = httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier);

                if (id is null || string.IsNullOrWhiteSpace(id.Value))
                {
                    throw new UnauthorizedAccessException("No se encontró el usuario autenticado.");
                }

                return Guid.Parse(id.Value);
            }
        }
        public string Email
        {
            get
            {
                return httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Email)?.Value
                    ?? throw new UnauthorizedAccessException("Email no encontrado.");
            }
        }
        public string Role
        {
            get
            {
                return httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Role)?.Value
                    ?? throw new UnauthorizedAccessException("Rol no encontrado.");
            }
        }
    }
}