
namespace Domain.Common
{
    public interface IAuditableEntity
    {
        long? CreatedByUserId { get; set; }
        DateTime? CreatedAt { get; set; }

        long? UpdatedByUserId { get; set; }
        DateTime? UpdatedAt { get; set; }

        long? DeletedByUserId { get; set; }
        DateTime? DeletedAt { get; set; }

        bool IsDeleted { get; set; }
    }
}
