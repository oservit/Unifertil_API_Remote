using Libs.Common;

namespace Application.Common
{
    public interface IAppServiceBase<T> : IAppSelectService<T>
    {
        Task<DataResult> Save(ICreateViewModel model);

        Task<DataResult> Update(long id, IUpdateViewModel model);

        Task<DataResult> Delete(long id);
    }
}