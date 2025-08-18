using Infrastructure.Services.Audit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Scope
{
    public class ServiceScope
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IAuditService, AuditService>();
        }
    }
}
