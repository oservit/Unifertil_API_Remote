using Application.Services.Auth;
using Application.Services.Base;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Clients
{
    public class ClientRemoteAppService : AuthenticatedAppService, IClientRemoteAppService
    {
        private readonly string _baseUrl;

        public ClientRemoteAppService(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config)
            : base(apiClient, tokenService)
        {
            _baseUrl = $"{config["Central:Url"].TrimEnd('/')}/Client";
        }

        public async Task<List<ClientRemoteDto>?> ListAllAsync()
            => (await GetAsync<ApiResponse<List<ClientRemoteDto>>>($"{_baseUrl}/ListAll"))?.Data;

        public async Task<ClientRemoteDto?> GetByIdAsync(long id)
            => (await GetAsync<ApiResponse<ClientRemoteDto>>($"{_baseUrl}/{id}"))?.Data;
    }
}
