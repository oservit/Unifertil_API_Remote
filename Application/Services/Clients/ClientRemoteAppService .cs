using Application.Services.Auth;
using Application.Services.Core;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Application.Services.Clients
{
    public class ClientRemoteAppService : AuthenticatedAppService, IClientRemoteAppService
    {
        private readonly RemoteCredentials _crendentials;
        private readonly string _baseUrl;

        public ClientRemoteAppService(
            IApiClient apiClient,
            ITokenService tokenService)
            : base(apiClient, tokenService)
        {
            _crendentials = new RemoteCredentials("CAR01", "9xIfOpsuIboSK/QHmdICcsysFqADJRPjxQFn0fKA3EI=", "http://localhost:50010/api");
            _baseUrl = $"http://localhost:50010/api/Client";
        }

        public async Task<List<ClientRemoteDto>?> ListAllAsync()
            => (await GetAsync<ApiResponse<List<ClientRemoteDto>>>($"{_baseUrl}/ListAll", _crendentials))?.Data;

        public async Task<ClientRemoteDto?> GetByIdAsync(long id)
            => (await GetAsync<ApiResponse<ClientRemoteDto>>($"{_baseUrl}/{id}", _crendentials))?.Data;
    }
}
