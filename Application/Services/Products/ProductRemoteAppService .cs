using Application.Services.Auth;
using Application.Services.Core;
using Infrastructure.Http;
using System.Net;

namespace Application.Services.Products
{
    public class ProductRemoteAppService : AuthenticatedAppService, IProductRemoteAppService
    {
        private readonly RemoteCredentials _crendentials;
        private readonly string _baseUrl;

        public ProductRemoteAppService(
            IApiClient apiClient,
            ITokenService tokenService)
            : base(apiClient, tokenService)
        {
            _crendentials = new RemoteCredentials("CAR010", "1234", "http://localhost:50010/api");
            _baseUrl = $"http://localhost:50010/api/Product";
        }

        public async Task<List<ProductRemoteDto>?> ListAllAsync()
            => (await GetAsync<ApiResponse<List<ProductRemoteDto>>>($"{_baseUrl}/ListAll", _crendentials))?.Data;

        public async Task<ProductRemoteDto?> GetByIdAsync(long id)
            => (await GetAsync<ApiResponse<ProductRemoteDto>>($"{_baseUrl}/{id}", _crendentials))?.Data;
    }
}
