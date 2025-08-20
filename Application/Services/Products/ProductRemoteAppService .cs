using Application.Services.Auth;
using Application.Services.Core;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Products
{
    public class ProductRemoteAppService : AuthenticatedAppService, IProductRemoteAppService
    {
        private readonly string _baseUrl;

        public ProductRemoteAppService(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config)
            : base(apiClient, tokenService)
        {
            _baseUrl = $"{config["Central:Url"].TrimEnd('/')}/Product";
        }

        public async Task<List<ProductRemoteDto>?> ListAllAsync()
            => (await GetAsync<ApiResponse<List<ProductRemoteDto>>>($"{_baseUrl}/ListAll"))?.Data;

        public async Task<ProductRemoteDto?> GetByIdAsync(long id)
            => (await GetAsync<ApiResponse<ProductRemoteDto>>($"{_baseUrl}/{id}"))?.Data;
    }
}
