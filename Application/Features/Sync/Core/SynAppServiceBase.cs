using Application.Common;
using Application.Common.Sync;
using AutoMapper;
using Domain.Common;
using Libs.Common;
using Libs.Exceptions;
using Service.Common;

namespace Application.Features.Sync.Core
{
    public abstract class SyncAppServiceBase<TEntity, TModel> : ISyncAppServiceBase<TModel>
        where TEntity : class, IEntityBase
        where TModel : class, IViewModelBase
    {
        protected readonly IServiceBase<TEntity> _service;
        protected readonly IMapper _mapper;

        protected SyncAppServiceBase(IServiceBase<TEntity> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public virtual async Task<DataResult> SyncLocal(SyncMessage<TModel> message)
        {
            try
            {
                if (message?.Payload == null)
                    throw new BusinessException("Payload nulo");

                if (!message.Payload.Id.HasValue)
                    throw new BusinessException("Valor do Id Não Informado");

                var entity = _mapper.Map<TEntity>(message.Payload);

                //await _service.SaveOrUpdate(entity);

                return new DataResult { Success = true, Data = entity };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }
    }
}
