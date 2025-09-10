using Application.Common;
using Application.Common.Sync;
using Application.Services.Auth;
using Application.Services.Core;
using Domain.Features.Sync;
using Domain.Features.Sync.Enums;
using Infrastructure.Http;
using Libs.Common;
using Service.Features.Sync;
using System.Text.Json;

namespace Application.Services.Sync.Core
{
    public abstract class SyncRemoteServiceBase<TModel> : AuthenticatedAppService, ISyncRemoteServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        private readonly ISyncLogService _logService;
        private readonly ISyncHashService _hashService;
        private readonly ISyncViewRouteUserService _routeService;

        protected SyncRemoteServiceBase(
            IApiClient apiClient,
            ITokenService tokenService,
            ISyncLogService logService,
            ISyncHashService hashService,
            ISyncViewRouteUserService routeService)
            : base(apiClient, tokenService)
        {
            _logService = logService;
            _hashService = hashService;
            _routeService = routeService;
        }

        protected abstract string GetRoute();

        /// <summary>
        /// Envia a mensagem para a remote correspondente ao ReceiverId
        /// </summary>
        public virtual async Task<DataResult> SyncRemote(SyncMessage<TModel> message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var syncLog = BuildSyncLog(message);
            var syncHash = BuildSyncHash(message);

            try
            {
                var credentials = await GetRemoteCredentials(message.Info.ReceiverId);
                var url = BuildSyncUrl(credentials.BaseUrl);

                var result = await PostAsync<DataResult>(url, message, credentials);

                if (message.Info.Caller.Equals(SyncCaller.Integrator))
                {
                    syncLog.Status = StatusEnum.Success;
                    await _logService.Save(syncLog);
                    await _hashService.SaveOrUpdate(syncHash);
                }

                return result;
            }
            catch (Exception ex)
            {
                if (message.Info.Caller.Equals(SyncCaller.Integrator))
                {
                    syncLog.Status = StatusEnum.Error;
                    syncLog.Message = ex.Message;

                    TrySaveLog(syncLog);
                }
                throw;
            }
        }

        private SyncLog BuildSyncLog(SyncMessage<TModel> message) =>
            new SyncLog
            {
                RecordId = message.Payload.Id.Value,
                LogDateTime = DateTime.Now,
                Payload = JsonSerializer.Serialize(message),
                HashValue = message.Info.Hash,
                Entity = (EntityEnum)message.Info.EntityId,
                Operation = (OperationEnum)message.Info.OperationId
            };

        private SyncHash BuildSyncHash(SyncMessage<TModel> message) =>
            new SyncHash
            {
                HashValue = message.Info.Hash,
                EntityId = message.Info.EntityId,
                RecordId = message.Payload.Id.Value,
                OperationId = message.Info.OperationId
            };

        private async Task<RemoteCredentials> GetRemoteCredentials(int receiverId)
        {
            try
            {
                var route = await _routeService.Get(x => x.TargetNodeId == receiverId && x.RouteIsActive && x.UserIsActive);

                if (route == null)
                    throw new Exception("Nenhuma Configuração de Rota Válida Encontrada");

                var info = new RemoteCredentials(route.UserName, "1234", route.TargetNodeUrl);

                return info;
            }

            catch
            {
                throw;
            }
        }

        private string BuildSyncUrl(string remoteUrl) => $"{remoteUrl}/{GetRoute()}/Sync/Receive";

        private async void TrySaveLog(SyncLog log)
        {
            try { await _logService.Save(log); }
            catch { /* swallow exception */ }
        }

    }
}
