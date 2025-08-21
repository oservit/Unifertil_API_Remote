using Application.Common;
using Application.Common.Sync;
using Libs.Common;

namespace Application.Features.Sync.Core
{
    public interface ISyncAppServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        /// <summary>
        /// Sincroniza localmente os dados recebidos
        /// </summary>
        Task<DataResult> SyncLocal(SyncMessage<TModel> message);
    }
}
