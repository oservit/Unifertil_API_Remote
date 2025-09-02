using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync
{
    [Table("SYNC_BATCHES")]
    public class SyncBatch : EntityBase
    {
        [Key]
        [Column("BATCH_ID")]
        public override long? Id { get; set; }

        [Column("START_TIME")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Column("END_TIME")]
        public DateTime? EndTime { get; set; }

        [Column("STATUS_ID")]
        [Required]
        public int StatusId { get; set; }

        [Column("MESSAGE")]
        [MaxLength(4000)]
        public string? Message { get; set; }
    }
}