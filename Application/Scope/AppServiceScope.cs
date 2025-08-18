using Application.Features.Authentication;
using Application.Services.Auth;
using Application.Services.Clients;
using Microsoft.Extensions.DependencyInjection;
using Service.Base;

namespace Application.Scope
{
    public class AppServiceScope
    {
        internal static void Register(IServiceCollection services)
        {
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<IClientRemoteAppService, ClientRemoteAppService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
