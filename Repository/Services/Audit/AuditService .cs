using Domain.Base;
using Infrastructure.Http;

namespace Infrastructure.Services.Audit
{
    public class AuditService: IAuditService
    {
        private readonly IHttpUserAccessor _httpUser;

        public AuditService(IHttpUserAccessor httpUser)
        {
            _httpUser = httpUser;
        }

        public void SetCreated<TEntity>(TEntity entity)
        {
            if (entity is IAuditableEntity baseEntity)
            {
                baseEntity.CreatedAt = DateTime.UtcNow;
                baseEntity.CreatedByUserId = _httpUser.User.Id;
            }
        }

        public void SetModified<TEntity>(TEntity entity)
        {
            if (entity is IAuditableEntity baseEntity)
            {
                baseEntity.UpdatedAt = DateTime.UtcNow;
                baseEntity.UpdatedByUserId = _httpUser.User.Id;
            }
        }

        public void SetDeleted<TEntity>(TEntity entity)
        {
            if (entity is IAuditableEntity baseEntity)
            {
                baseEntity.DeletedAt = DateTime.UtcNow;
                baseEntity.DeletedByUserId = _httpUser.User.Id;
                baseEntity.IsDeleted = true;
            }
        }
    }
}
