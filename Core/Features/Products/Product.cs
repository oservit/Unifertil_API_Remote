using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Products
{
    [Table("PRODUCTS")]
    public class Product : EntityBase
    {
        [Key]
        [Column("ID")]
        [Sequence("SEQ_PRODUCTS_ID")]
        public override long? Id { get; set; }

        [Column("NAME")]
        public string? Name { get; set; }

        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("CATEGORY")]
        public string? Category { get; set; }

        [Column("UNIT_PRICE", TypeName = "NUMBER(10,2)")]
        public decimal? UnitPrice { get; set; }

        [Column("STOCK_QUANTITY")]
        public int? StockQuantity { get; set; }

        [Column("UNIT_OF_MEASURE")]
        public string? UnitOfMeasure { get; set; }

        [Column("MANUFACTURER")]
        public string? Manufacturer { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
    }
}
