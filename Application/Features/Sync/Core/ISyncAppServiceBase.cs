using Application.Common;
using Libs.Common;

namespace Application.Features.Sync.Core
{
    public interface ISyncRemoteAppServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        /// <summary>
        /// Insere ou atualiza o registro com base no Id do model.
        /// Retorna DataResult com sucesso ou falha.
        /// </summary>
        Task<DataResult> Sync(TModel model);
    }
}
