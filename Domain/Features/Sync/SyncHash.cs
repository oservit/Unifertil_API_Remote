using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync
{
    [Table("SYNC_HASHES")]
    public class SyncHash : EntityBase
    {
        [Key]
        [Column("ID")]
        public override long? Id { get; set; }

        [Column("HASH_VALUE")]
        [Required]
        [MaxLength(256)]
        public string HashValue { get; set; } = string.Empty;

        [Column("ENTITY_ID")]
        [Required]
        public int EntityId { get; set; }

        [Column("RECORD_ID")]
        [Required]
        public long RecordId { get; set; }

        [Column("OPERATION_ID")]
        [Required]
        public long OperationId { get; set; }


    [Column("OPERATION_DATE")]
        public DateTime OperationDate { get; set; } = DateTime.Now;
    }
}
