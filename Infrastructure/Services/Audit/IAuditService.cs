namespace Infrastructure.Services.Audit
{
    public interface IAuditService
    {
        void SetCreated<TEntity>(TEntity entity);
        void SetModified<TEntity>(TEntity entity);
        void SetDeleted<TEntity>(TEntity entity);
    }
}
