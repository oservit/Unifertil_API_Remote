using Application.Services.Auth;
using Infrastructure.Http;

namespace Application.Services.Core
{
    public abstract class AuthenticatedAppService
    {
        protected readonly IApiClient ApiClient;
        private readonly ITokenService _tokenService;

        protected AuthenticatedAppService(IApiClient apiClient, ITokenService tokenService)
        {
            ApiClient = apiClient;
            _tokenService = tokenService;
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            var token = await _tokenService.GetTokenAsync();
            return await ApiClient.GetAsync<T>(url, token);
        }

        protected async Task<T?> PostAsync<T>(string url, object data)
        {
            var token = await _tokenService.GetTokenAsync();
            return await ApiClient.PostAsync<T>(url, data, token);
        }
    }
}
