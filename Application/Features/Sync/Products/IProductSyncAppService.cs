using Application.Features.Products;
using Application.Features.Sync.Core;
using Domain.Features.Products;

namespace Application.Features.Sync.Products
{
    public interface IProductSyncAppService : ISyncRemoteAppServiceBase<ProductViewModel>
    {
    }
}
