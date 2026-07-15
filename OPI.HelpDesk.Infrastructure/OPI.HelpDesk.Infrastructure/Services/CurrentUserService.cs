using Microsoft.AspNetCore.Http;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using System.Security.Claims;

namespace OPI.HelpDesk.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public Guid? UserId
        {
            get
            {
                var id = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                return (id == null || string.IsNullOrEmpty(id.Value))? null : Guid.Parse(id.Value);
            }
        }
    }
}
