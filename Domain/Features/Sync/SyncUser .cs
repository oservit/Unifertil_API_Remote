using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync
{
    [Table("SYNC_USERS")]
    public class SyncUser : EntityBase
    {
        [Key]
        [Column("ID")]
        public override long? Id { get; set; }

        [Column("NODE_ID")]
        [Required]
        public long NodeId { get; set; }

        [Column("USERNAME")]
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Column("PASSWORD")]
        [Required]
        [MaxLength(500)]
        public string Password { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("UPDATED_AT")]
        public DateTime? UpdatedAt { get; set; }
    }
}
