using Domain.Common;
using Domain.Features.Authentication.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Authentication
{
    [Table("API_USERS")]
    public class User : EntityBase
    {
        [Key]
        [Column("ID")]
        public override long? Id { get; set; }

        [Column("TYPE_ID")]
        [Required]
        public UserTypeEnum TypeId { get; set; }

        [Column("USERNAME")]
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Column("PASSWORD")]
        [Required]
        [MaxLength(500)]
        public string Password { get; set; } = string.Empty;

        [Column("DESCRIPTION")]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; } = true;

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("UPDATED_AT")]
        public DateTime? UpdatedAt { get; set; }
    }
}
