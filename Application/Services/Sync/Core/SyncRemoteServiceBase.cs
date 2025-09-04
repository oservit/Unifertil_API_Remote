using Application.Common;
using Application.Common.Sync;
using Application.Services.Auth;
using Application.Services.Core;
using Domain.Features.Sync;
using Domain.Features.Sync.Enums;
using Infrastructure.Http;
using Libs.Common;
using Libs.Exceptions;
using Microsoft.Extensions.Configuration;
using Service.Features.Sync;
using System.Text.Json;

namespace Application.Services.Sync.Core
{
    public abstract class SyncRemoteServiceBase<TModel> : AuthenticatedAppService, ISyncRemoteServiceBase<TModel>
        where TModel : class, IViewModelBase
    {
        private readonly IConfiguration _config;
        private readonly ISyncLogService _logService;
        private readonly ISyncHashService _hashService;

        protected SyncRemoteServiceBase(
            IApiClient apiClient,
            ITokenService tokenService,
            IConfiguration config,
            ISyncLogService logService,
            ISyncHashService hashService)
            : base(apiClient, tokenService, config)
        {
            _config = config; ;
            _logService = logService;
            _hashService = hashService;
        }

        protected abstract string GetRoute();

        /// <summary>
        /// Envia a mensagem para a remote correspondente ao ReceiverId
        /// </summary>
        public virtual async Task<DataResult> SyncRemote(SyncMessage<TModel> message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var syncLog = new SyncLog
            {
                RecordId = message.Payload.Id.Value,
                LogDateTime = DateTime.Now,
                Payload = JsonSerializer.Serialize(message),
                HashValue = message.Info.Hash,
                Entity = (EntityEnum)message.Info.EntityId,
                Operation = (OperationEnum)message.Info.OperationId
            };

            var syncHash = new SyncHash
            {
                HashValue = message.Info.Hash,
                EntityId = message.Info.EntityId,
                RecordId = message.Payload.Id.Value,
                OperationId = message.Info.OperationId
            };

            try
            {
                var remote = _config.GetSection("Remotes")
                                    .GetChildren()
                                    .Select(r => new
                                    {
                                        Id = int.Parse(r["Id"]!),
                                        Url = r["Url"]?.TrimEnd('/'),
                                        User = r["User"],
                                        Password = r["Password"]
                                    })
                                    .FirstOrDefault(r => r.Id == message.Info.ReceiverId);

                if (remote == null || string.IsNullOrEmpty(remote.Url))
                    throw new InvalidOperationException($"Remote com Id {message.Info.ReceiverId} não encontrado ou sem URL");

                var url = $"{remote.Url}/{GetRoute()}/Sync/Receive";
                var credentiais = new RemoteCredentials(remote.User, remote.Password, remote.Url);

                var result = await PostAsync<DataResult>(url, message, credentiais);

                // Só grava log e hash se for uma chamada via integrador.
                if (message.Info.Caller.Equals(SyncCaller.Integrator))
                {
                    syncLog.Status = StatusEnum.Success;
                    syncLog.Message = null;

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

                    try
                    {
                        await _logService.Save(syncLog);
                    }
                    catch
                    {
                    }
                }
                throw;
            }
        }
    }
}
