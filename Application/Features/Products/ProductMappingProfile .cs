using AutoMapper;
using Domain.Features.Products;

namespace Application.Features.Products.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductModel, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ProductViewModel, Product>();
            CreateMap<UpdateProductModel, Product>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
