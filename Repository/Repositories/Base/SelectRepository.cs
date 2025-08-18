using Domain.Base;
using Libs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Base
{
    public class SelectRepository<T> : ISelectRepository<T> where T : class
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public SelectRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> ApplyExclusionFilter(IQueryable<T> query)
        {
            if (typeof(IAuditableEntity).IsAssignableFrom(typeof(T)))
                query = query.Where(e => EF.Property<int>(e, "IsDeleted") == 0);

            return query;
        }

        public IQueryable<T> ApplyDefaultOrdering(IQueryable<T> query)
        {
            var keyProperty = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties
                .FirstOrDefault()?.Name;

            if (keyProperty != null)
                query = query.OrderBy(e => EF.Property<object>(e, keyProperty));

            return query;
        }


        public IQueryable<T> CreateQuery()
        {
            var query = _dbSet.AsQueryable();

            query = ApplyExclusionFilter(query);

            return query;
        }

        public virtual IQueryable<T> ApplyFilter(IQueryable<T> query, FilterRequest? filterRequest = null)
        {
            query = ApplyExclusionFilter(query);
            if (filterRequest != null && FilterBuilder.ValidateFilter(filterRequest))
                query = FilterBuilder.BuildFilter(query, filterRequest);

            return ApplyDefaultOrdering(query);
        }

        public virtual IQueryable<T> ApplyOrdering(IQueryable<T> query, OrderByRequest? orderByRequest = null)
        {
            return orderByRequest != null && orderByRequest.ValidateRequest() ? query.ApplyOrdering(orderByRequest) : ApplyDefaultOrdering(query);
        }

        public virtual async Task<T?> Get(long id)
        {
            return await ApplyExclusionFilter(_context.Set<T>())
                .FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }

        public virtual async Task<T?> Get(string id)
        {
            return await ApplyExclusionFilter(_context.Set<T>())
                .FirstOrDefaultAsync(e => EF.Property<string>(e, "Id") == id);
        }

        public virtual async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            return await ApplyExclusionFilter(_context.Set<T>())
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TResult?> GetField<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector)
        {
            return await ApplyExclusionFilter(_context.Set<T>())
                .AsNoTracking()
                .Where(predicate)
                .Select(selector)
                .FirstOrDefaultAsync();
        }


        public virtual async Task<IEnumerable<T>> GetList(FilterRequest? filterRequest = null)
        {
            IQueryable<T> query = ApplyExclusionFilter(_dbSet);
            query = IncludeNavigationProperties(query);
            query = ApplyFilter(query, filterRequest);
            return await query.AsNoTracking().Distinct().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate)
        {
            return await ApplyDefaultOrdering(
                ApplyExclusionFilter(_context.Set<T>()).Where(predicate)
            )
            .AsNoTracking()
            .Distinct()
            .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        {
            return await ApplyDefaultOrdering(
                ApplyExclusionFilter(_context.Set<T>()).Where(predicate)
            )
            .AsNoTracking()
            .Distinct()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetList(int pageIndex, int pageSize)
        {
            return await ApplyDefaultOrdering(ApplyExclusionFilter(_context.Set<T>()))
                .AsNoTracking()
                .Distinct()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<long> Count(Expression<Func<T, bool>> predicate)
        {
            return await ApplyExclusionFilter(_context.Set<T>()).CountAsync(predicate);
        }

        public virtual async Task<long> Count()
        {
            return await ApplyExclusionFilter(_context.Set<T>()).CountAsync();
        }

        public virtual bool Exists(long id)
        {
            return ApplyExclusionFilter(_context.Set<T>())
                .AsNoTracking()
                .Any(e => EF.Property<long>(e, "Id") == id);
        }

        public virtual async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await ApplyExclusionFilter(_context.Set<T>())
                .AsNoTracking()
                .AnyAsync(predicate);
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }

        public virtual async Task<IEnumerable<T>> GetPagedResults(FilterRequest filterRequest, OrderByRequest? orderByRequest, int pageIndex, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            query = ApplyExclusionFilter(query);

            query = ApplyFilter(query, filterRequest);

            query = ApplyOrdering(query, orderByRequest);
            var sqlQuery = query.ToQueryString();
            Console.WriteLine("SQL Query: " + sqlQuery);

            return await query
                .AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        private IQueryable<T> IncludeNavigationProperties(IQueryable<T> query)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            if (entityType != null)
            {
                var navigationProperties = entityType.GetNavigations();
                foreach (var navigationProperty in navigationProperties)
                {
                    var propertyName = navigationProperty.Name;
                    query = query.Include(propertyName);
                }
            }
            return query;
        }
    }
}
