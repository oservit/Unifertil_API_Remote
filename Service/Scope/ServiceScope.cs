using Microsoft.Extensions.DependencyInjection;
using Service.Features.Authentication;
using Service.Features.Products;
using Service.Features.Sync;

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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISyncLogService, SyncLogService>();
            services.AddScoped<ISyncHashService, SyncHashService>();
            services.AddScoped<ISyncBatchService, SyncBatchService>();
        }
    }
}
