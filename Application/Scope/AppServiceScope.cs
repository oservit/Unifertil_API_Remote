using Application.Features.Authentication;
using Application.Services.Auth;
using Application.Services.Clients;
using Application.Services.Products;
using Microsoft.Extensions.DependencyInjection;
using Service.Common;

namespace Application.Scope
{
    public class AppServiceScope
    {
        internal static void Register(IServiceCollection services)
        {
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<IClientRemoteAppService, ClientRemoteAppService>();
            services.AddScoped<IProductRemoteAppService, ProductRemoteAppService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
