using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using OPI.HelpDesk.Application.Interfaces.Repositories;
using OPI.HelpDesk.Application.Interfaces.Security;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Infrastructure.Persistence;
using OPI.HelpDesk.Infrastructure.Persistence.Repositories;
using OPI.HelpDesk.Infrastructure.Persistence.UnitOfWorks;
using OPI.HelpDesk.Infrastructure.Services;
using OPI.HelpDesk.Infrastructure.Services.Security;

namespace OPI.HelpDesk.Infrastructure.DI
{
    public static class InfraestructureDependencyInjection
    {
        public static IServiceCollection AddInfraestructureService(this IServiceCollection services, IConfiguration conf)
        {
            string connectionString = conf.GetConnectionString("DefaultConnection");
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddDbContext<HelpDeskDbContext>(config =>
            {
                config.UseNpgsql(connectionString);
            });
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
