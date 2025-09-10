using Application.Features.Products;
using Application.Services.Auth;
using Application.Services.Sync.Core;
using Infrastructure.Http;
using Service.Features.Sync;

namespace Application.Services.Sync.Products
{
    public class ProductSyncRemoteService : SyncRemoteServiceBase<ProductViewModel>, IProductSyncRemoteService
    {
        public ProductSyncRemoteService(
            IApiClient apiClient,
            ITokenService tokenService,
            ISyncLogService logService,
            ISyncHashService hashService,
            ISyncViewRouteUserService routeService)
            : base(apiClient, tokenService, logService, hashService, routeService)
        {
        }
        protected override string GetRoute() => "Product";
    }
}
