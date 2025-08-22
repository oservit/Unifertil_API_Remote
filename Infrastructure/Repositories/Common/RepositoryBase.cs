using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Common
{
    public class RepositoryBase<T> : SelectRepository<T>, IRepositoryBase<T> where T : class, IEntityBase
    {
        public RepositoryBase(DbContext context) : base(context) { }

        public virtual async Task<int> Save(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _context.Set<T>().Add(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> SaveList(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            _context.Set<T>().AddRange(entities);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var tracked = await _context.Set<T>().FindAsync(entity.Id);

            if (tracked == null)
            {
                _context.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
                _context.Entry(tracked).CurrentValues.SetValues(entity);

            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateSomeFields(long id, object updatedFields)
        {
            var entity = Activator.CreateInstance<T>();
            var entry = _context.Entry(entity);

            entry.Property("Id").CurrentValue = id;
            _context.Attach(entity);

            var props = updatedFields.GetType().GetProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(updatedFields);
                entry.Property(prop.Name).CurrentValue = value;
                entry.Property(prop.Name).IsModified = true;
            }

            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> SoftDelete(IAuditableEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
                _context.Attach(entity);

            entity.DeletedAt = DateTime.UtcNow;
            entity.IsDeleted = true;

            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> Delete(long id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                throw new InvalidOperationException("Entidade não encontrada.");

            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
