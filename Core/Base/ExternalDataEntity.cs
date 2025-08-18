using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Base
{
    public class ExternalDataEntity : EntityBase, IExternalDataEntity
    {
        [IgnoreOnUpdate]
        [Column("ERP_CODE", TypeName = "VARCHAR2(32)")]
        public string? ErpCode { get; set; } = null;
    }
}
