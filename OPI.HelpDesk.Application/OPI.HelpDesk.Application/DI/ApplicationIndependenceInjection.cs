using Microsoft.Extensions.DependencyInjection;
using OPI.HelpDesk.Application.Interfaces.Security;
using OPI.HelpDesk.Application.Interfaces.SettingSLAs;
using OPI.HelpDesk.Application.Interfaces.Users;
using OPI.HelpDesk.Application.Services.Security;
using OPI.HelpDesk.Application.Services.SettingSLAs;
using OPI.HelpDesk.Application.Services.Users;

namespace OPI.HelpDesk.Application.DI
{
    public static class ApplicationIndependenceInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection service)
        {
            service.AddTransient<ISecurityService, SecurityService>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<ISettingSLA, SettingSLAService>();
            return service;
        }
    }
}
