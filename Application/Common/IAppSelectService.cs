using Libs;
using Libs.Common;

namespace Application.Common
{
    public interface IAppSelectService<T>
    {
        Task<DataResult> Get(string id);
        Task<DataResult> Get(long id);

        Task<DataResult> GetList(FilterRequest? filter = null);

        Task<DataPagedResult> GetListPaged(int pageIndex = 1, int pageSize = 20);

        Task<DataPagedResult> GetPagedResults(FilterRequest filter, OrderByRequest order, int pageIndex = 1, int pageSize = 20);

    }
}
