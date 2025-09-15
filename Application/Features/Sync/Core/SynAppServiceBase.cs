using Application.Common;
using Application.Common.Sync;
using AutoMapper;
using Domain.Common;
using Domain.Features.Sync;
using Domain.Features.Sync.Enums;
using Libs.Common;
using Libs.Exceptions;
using Service.Common;
using Service.Features.Sync;
using System.Text.Json;

namespace Application.Features.Sync.Core
{
    public abstract class SyncAppServiceBase<TEntity, TModel> : ISyncAppServiceBase<TModel>
        where TEntity : class, IEntityBase
        where TModel : class, IViewModelBase
    {
        protected readonly IServiceBase<TEntity> _service;
        private readonly ISyncLogService _logService;
        private readonly ISyncHashService _hashService;
        protected readonly IMapper _mapper;

        protected SyncAppServiceBase(IServiceBase<TEntity> service, ISyncLogService logService, ISyncHashService hashService, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            _logService = logService;
            _hashService = hashService;
        }

        public virtual async Task<DataResult> SyncLocal(SyncMessage<TModel> message)
        {
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
                if (message?.Payload == null)
                    throw new BusinessException("Payload nulo");

                if (!message.Payload.Id.HasValue)
                    throw new BusinessException("Valor do Id Não Informado");

                var entity = _mapper.Map<TEntity>(message.Payload);

                // Primeiro Salvar o Hash
                await _hashService.SaveOrUpdate(syncHash);
                await _service.SaveOrUpdate(entity);

                syncLog.Status = StatusEnum.Success;
                syncLog.Message = null;

                await _logService.Save(syncLog);

                return new DataResult { Success = true, Data = entity };
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
