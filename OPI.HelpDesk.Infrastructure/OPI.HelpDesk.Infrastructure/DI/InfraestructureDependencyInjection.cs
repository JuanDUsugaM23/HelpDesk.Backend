using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using OPI.HelpDesk.Infrastructure.Persistence;
using OPI.HelpDesk.Infrastructure.Services;

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
            return services;
        }
    }
}
