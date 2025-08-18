using Libs;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Base;
using System.Linq.Expressions;

namespace Service.Base
{
    public class SelectService<T> : IDisposable, ISelectService<T> where T : class
    {
        protected readonly ISelectRepository<T> _repository;

        public SelectService(ISelectRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<dynamic>> DbQuery(
           Func<IQueryable<T>, IQueryable<dynamic>> projection,
           bool useDistinct = false,
           FilterRequest? filter = null,
           OrderByRequest? orderBy = null,
           int? pageIndex = null,
           int? pageSize = null,
           params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));

            IQueryable<T> query = _repository.CreateQuery();

            foreach (var include in includes)
            {
                if (include != null)
                {
                    query = include(query);
                }
            }

            if (filter != null && FilterBuilder.ValidateFilter(filter))
                query = FilterBuilder.BuildFilter(query, filter);

            if (orderBy != null && orderBy.ValidateRequest())
            {
                query = _repository.ApplyOrdering(query, orderBy);
            }
            else if ((pageIndex.HasValue || pageSize.HasValue) && typeof(T).GetProperty("Id") != null)
            {
                query = query.OrderBy(e => EF.Property<object>(e, "Id"));
            }
            else if (pageIndex.HasValue || pageSize.HasValue)
            {
                var firstProp = typeof(T)
                    .GetProperties()
                    .FirstOrDefault(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string));

                if (firstProp != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "e");
                    var propertyAccess = Expression.Property(parameter, firstProp.Name);
                    var converted = Expression.Convert(propertyAccess, typeof(object));
                    var lambda = Expression.Lambda<Func<T, object>>(converted, parameter);

                    query = query.OrderBy(lambda);
                }
            }

            var projectedQuery = projection(query);

            if (useDistinct)
                projectedQuery = projectedQuery.Distinct();

            projectedQuery = projectedQuery.ApplyPagination(pageIndex, pageSize);

            return await projectedQuery.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetList(
            Func<IQueryable<T>, IQueryable<dynamic>> projection,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _repository.CreateQuery();

            if (filter != null)
                query = query.Where(filter);

            var projectedQuery = projection(query);

            // Remover duplicatas
            return await projectedQuery.Distinct().ToListAsync();
        }

        public async Task<dynamic?> Get(
           Func<IQueryable<T>, IQueryable<dynamic>> projection,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _repository.CreateQuery();

            if (filter != null)
                query = query.Where(filter);

            return await projection(query).FirstOrDefaultAsync();
        }

        public virtual async Task<long> Count()
        {
            return await this._repository.Count();
        }

        public virtual async Task<long> Count(Expression<Func<T, bool>> predicate)
        {
            return await this._repository.Count(predicate);
        }

        public virtual async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            return await this._repository.Get(predicate);
        }

        public virtual async Task<T?> Get(long id)
        {
            return await this._repository.Get(id);
        }

        public virtual async Task<T?> Get(string id)
        {
            return await this._repository.Get(id);
        }

        public virtual async Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate)
        {
            return await this._repository.GetList(predicate);
        }

        public virtual async Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        {
            return await this._repository.GetList(predicate, pageIndex, pageSize);
        }

        public virtual async Task<IEnumerable<T>> GetList(int pageIndex, int pageSize)
        {
            return await this._repository.GetList(pageIndex, pageSize);
        }

        public virtual async Task<IEnumerable<T>> GetPagedResults(FilterRequest filter, OrderByRequest order, int pageIndex, int pageSize)
        {
            return await this._repository.GetPagedResults(filter, order, pageIndex, pageSize);
        }

        public virtual bool Exists(long id)
        {
            return this._repository.Exists(id);
        }

        public virtual void Dispose()
        {
            this._repository.Dispose();
        }

        public async Task<IEnumerable<T>> GetList(FilterRequest? filterRequest = null)
        {
            return await this._repository.GetList(filterRequest);
        }
    }
}