using Application.Features.Products;
using Application.Services.Auth;
using Application.Services.Sync.Core;
using Infrastructure.Http;
using Microsoft.Extensions.Configuration;
using Service.Features.Sync;

namespace Application.Services.Sync.Products
{
    public class ProductSyncRemoteService : SyncRemoteServiceBase<ProductViewModel>, IProductSyncRemoteService
    {
        public ProductSyncRemoteService(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config,
            ISyncLogService logService)
            : base(apiClient, tokenService, config, logService)
        {
        }
        protected override string GetRoute() => "Product";
    }
}
