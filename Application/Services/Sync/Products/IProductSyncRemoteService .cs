using Application.Features.Products;
using Application.Services.Sync.Core;

namespace Application.Services.Sync.Products
{
    public interface IProductSyncRemoteService : ISyncRemoteServiceBase<ProductViewModel>
    {
    }
}
