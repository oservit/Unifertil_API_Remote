using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync
{
    [Table("SYNC_ROUTES")]
    public class SyncRoute : EntityBase
    {
        [Key]
        [Column("ID")]
        public override long? Id { get; set; }

        [Column("SOURCE_NODE_ID")]
        [Required]
        public long SourceNodeId { get; set; }

        [Column("TARGET_NODE_ID")]
        [Required]
        public long TargetNodeId { get; set; }

        [Column("USER_ID")]
        [Required]
        public long UserId { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("UPDATED_AT")]
        public DateTime? UpdatedAt { get; set; }
    }
}
