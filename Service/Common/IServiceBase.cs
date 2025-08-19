using System.Threading.Tasks;
using Domain.Common;

namespace Service.Common
{
    public interface IServiceBase<T> : ISelectService<T> where T : class, IEntityBase
    {
        Task<int> Save(T model);

        Task<int> Update(T model);

        Task<int> Delete(long id);
    }
}