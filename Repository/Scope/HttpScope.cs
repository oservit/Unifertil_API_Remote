using Infrastructure.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Scope
{
    public class HttpScope
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IHttpUserAccessor, HttpUserAccessor>();
            services.AddScoped<IApiClient, ApiClient>();
        }
    }
}
