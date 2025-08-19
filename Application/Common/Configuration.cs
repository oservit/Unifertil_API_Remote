using Application.Scope;
using Infrastructure.Scope;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common
{
    public class Configuration
    {
        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(IServiceCollection services)
        {
            AppServiceScope.Register(services);

            Service.Scope.ServiceScope.Register(services);

            InfrastrucureScope.Register(services);
        }
    }
}
