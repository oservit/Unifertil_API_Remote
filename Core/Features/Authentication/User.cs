using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Authentication
{
    [Table("USERS")]
    public class User: EntityBase
    {
        [Key]
        [Column("ID")]
        [Sequence("SEQ_USERS_ID")]
        public override long? Id { get; set; }

        [Column("USERNAME")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Column("EMAIL")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("IS_ACTIVE")]
        [BoolAsInt]
        public bool IsActive { get; set; }
    }
}
