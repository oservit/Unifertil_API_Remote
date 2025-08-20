using Application.Common;
using Libs.Common;

namespace Application.Services.Sync.Core
{
    public interface ISyncRemoteServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        /// <summary>
        /// Insere ou atualiza o registro com base no Id do model.
        /// Retorna DataResult com sucesso ou falha.
        /// </summary>
        Task<DataResult> SyncRemote(TModel model);
    }
}
