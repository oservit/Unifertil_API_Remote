using Application.Common;
using AutoMapper;
using Domain.Features.Products;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Service.Features.Products;

namespace Application.Features.Products
{
    public class ProductAppService : AppServiceBase<Product, ProductViewModel>, IProductAppService
    {
        public ProductAppService(IProductService service, IMapper mapper, IOptions<AppSettings> settings) : base(service, mapper, settings)
        {
        }
    }
}
