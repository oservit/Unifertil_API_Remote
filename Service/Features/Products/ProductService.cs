using Domain.Features.Products;
using Infrastructure.Repositories.Products;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;

namespace Service.Features.Products
{
    public class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IProductRepositoryRepository _productRepository;
        private readonly IHttpUserAccessor _httpUser;
        private readonly IAuditService _auditService;

        public ProductService(IProductRepositoryRepository productRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(productRepository, httpUser, auditService)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _httpUser = httpUser ?? throw new ArgumentNullException(nameof(httpUser));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }
    }
}
