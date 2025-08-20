using Application.Features.Products;
using Application.Services.Auth;
using Application.Services.Sync.Core;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Sync.Products
{
    public class ProductSyncRemoteService : SyncRemoteServiceBase<ProductViewModel>, IProductSyncRemoteService
    {
        public ProductSyncRemoteService(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config)
            : base(apiClient, tokenService, config, "Product")
        {
        }
    }
}
