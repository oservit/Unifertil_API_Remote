using Application.Common;
using Application.Common.Sync;
using Libs.Common;

namespace Application.Services.Sync.Core
{
    public interface ISyncRemoteServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        /// <summary>
        /// Envia dados para sincronização remota
        /// </summary>
        Task<DataResult> SyncRemote(SyncMessage<TModel> message);
    }
}
