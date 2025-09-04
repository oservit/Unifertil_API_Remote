using Application.Features.Products;
using Application.Features.Sync.Core;
using AutoMapper;
using Domain.Features.Products;
using Service.Features.Products;
using Service.Features.Sync;

namespace Application.Features.Sync.Products
{
    public class ProductSyncAppService : SyncAppServiceBase<Product, ProductViewModel>, IProductSyncAppService
    {
        public ProductSyncAppService(IProductService service, ISyncLogService logService, ISyncHashService hashService, IMapper mapper)
            : base(service, logService, hashService, mapper)
        {
        }
    }
}
