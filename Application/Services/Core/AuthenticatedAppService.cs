using Application.Services.Auth;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Core
{
    public abstract class AuthenticatedAppService
    {
        protected readonly IApiClient ApiClient;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        protected AuthenticatedAppService(IApiClient apiClient, ITokenService tokenService, IConfiguration configuration)
        {
            ApiClient = apiClient;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        protected async Task<T?> GetAsync<T>(string url, RemoteCredentials? credentials = null)
        {
            credentials ??= GetDefaultCredentials();
            var token = await _tokenService.GetTokenAsync(credentials);
            return await ApiClient.GetAsync<T>(url, token);
        }

        protected async Task<T?> PostAsync<T>(string url, object data, RemoteCredentials? credentials = null)
        {
            credentials ??= GetDefaultCredentials();
            var token = await _tokenService.GetTokenAsync(credentials);
            return await ApiClient.PostAsync<T>(url, data, token);
        }

        private RemoteCredentials GetDefaultCredentials()
        {
            var first = _configuration.GetSection("Remotes")
                               .GetChildren()
                               .FirstOrDefault();

            if (first == null)
                throw new InvalidOperationException("Nenhuma configuração de 'Remotes' encontrada no appsettings.json");

            return new RemoteCredentials
            (
               first["User"]!,
               first["Password"]!,
               first["Url"]!
            );
        }
    }
}
