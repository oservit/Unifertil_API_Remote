using Libs.Common;
using Libs;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Common;

namespace Service.Common
{
    public class ViewService<T> : IViewService<T> where T : class
    {
        protected readonly ISelectRepository<T> _repository;

        public ViewService(ISelectRepository<T> repository)
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

            query = query.ApplyOrdering(orderBy);

            query = query.ApplyPagination(pageIndex, pageSize);

            IQueryable<dynamic> projectedQuery = projection(query);

            if (useDistinct)
                return await projectedQuery.Distinct().ToListAsync();

            return await projectedQuery.ToListAsync();
        }

        public async Task<DataPagedResult> GetPaged(FilterRequest filter, OrderByRequest order, int pageIndex, int pageSize)
        {
            var result = await _repository.GetPagedResults(filter, order, pageIndex, pageSize);
            var totalCount = await _repository.Count();

            return new DataPagedResult
            {
                Data = result,
                Success = true,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public virtual async Task<long> Count()
        {
            return await this._repository.Count();
        }
    }
}
