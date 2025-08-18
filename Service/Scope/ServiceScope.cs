using Microsoft.Extensions.DependencyInjection;
using Service.Features.Authentication;

namespace Service.Scope
{
    public class ServiceScope
    {
        /// <summary>
        /// Registra os Serviços.
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
