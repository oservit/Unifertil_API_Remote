using Domain.Common;
using Domain.Features.Sync.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync
{
    [Table("SYNC_NODES")]
    public class SyncNode : EntityBase
    {
        [Key]
        [Column("ID")]
        public override long? Id { get; set; }

        [Column("NAME")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("URL")]
        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = string.Empty;

        [Column("TYPE_ID")]
        [Required]
        public NodeTypeEnum Type { get; set; }

        [Column("IS_ACTIVE")]
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
