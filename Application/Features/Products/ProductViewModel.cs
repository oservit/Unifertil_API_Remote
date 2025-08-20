using Application.Common;

namespace Application.Features.Products
{
    public class ProductViewModel: ViewModelBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? StockQuantity { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Manufacturer { get; set; }
    }

    public class CreateProductModel: CreateViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }

    public class UpdateProductModel: UpdateViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


}
