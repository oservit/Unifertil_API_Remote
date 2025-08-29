using Domain.Common;
using Domain.Features.Sync.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync
{
    [Table("SYNC_LOG")]
    public class SyncLog : EntityBase
    {
        [Key]
        [Column("LOG_ID")]
        public override long? Id { get; set; }

        [Column("ENTITY_ID")]
        [Required]
        public EntityEnum Entity{ get; set; }

        [Column("RECORD_ID")]
        [Required]
        public long RecordId { get; set; }

        [Column("STATUS_ID")]
        [Required]
        public StatusEnum Status { get; set; }

        [Column("OPERATION_ID")]
        [Required]
        public OperationEnum Operation { get; set; }

        [Column("MESSAGE")]
        [MaxLength(4000)]
        public string? Message { get; set; }

        [Column("LOG_DATETIME")]
        public DateTime? LogDateTime { get; set; } = DateTime.Now;

        [Column("PAYLOAD", TypeName = "CLOB")]
        public string? Payload { get; set; }

        [Column("HASH_VALUE")]
        [Required]
        [MaxLength(256)]
        public string HashValue { get; set; } = string.Empty;
    }
}
