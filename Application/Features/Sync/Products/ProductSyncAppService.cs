using Application.Features.Products;
using Application.Features.Sync.Core;
using AutoMapper;
using Domain.Features.Products;
using Service.Features.Products;

namespace Application.Features.Sync.Products
{
    public class ProductSyncAppService : SyncAppServiceBase<Product, ProductViewModel>, IProductSyncAppService
    {
        public ProductSyncAppService(IProductService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
