using Libs;
using Libs.Base;

namespace Service.Base
{
    public interface IViewService<T> where T : class
    {
        public Task<IEnumerable<dynamic>> DbQuery(
            Func<IQueryable<T>, IQueryable<dynamic>> projection,
            bool useDistinct = false,
            FilterRequest? filter = null,
            OrderByRequest? orderBy = null,
            int? pageIndex = null,
            int? pageSize = null,
            params Func<IQueryable<T>, IQueryable<T>>[] includes);

        Task<DataPagedResult> GetPaged(FilterRequest filter, OrderByRequest order, int pageIndex, int pageSize);

        Task<long> Count();
    }
}
