using AutoMapper;
using Domain.Common;
using Domain.Settings;
using Libs.Base;
using Libs.Exceptions;
using Microsoft.Extensions.Options;
using Service.Common;

namespace Application.Common
{
    public class AppServiceBase<T, TModel> : AppSelectService<T>, IAppServiceBase<TModel> where T : class, IEntityBase
    {
        protected readonly new IServiceBase<T> _service;
        protected readonly IMapper _mapper;
        private readonly AppSettings _settings;
        public AppServiceBase(IServiceBase<T> service, IMapper mapper, IOptions<AppSettings> settings) : base(service)
        {
            _service = service;
            _mapper = mapper;
            _settings = settings.Value;
        }

        public virtual async Task<DataResult> Save(ICreateViewModel model)
        {
            try
            {
                var entity = _mapper.Map<T>(model);
                var validationErrors = GenericValidator.Validate(entity);

                if (validationErrors.Any())
                    return new DataResult
                    {
                        Success = false,
                        Message = string.Join(Environment.NewLine, validationErrors)
                    };
                await _service.Save(entity);

                return new DataResult
                {
                    Success = true,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

        public virtual async Task<DataResult> Update(long id, IUpdateViewModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Valores não repassados ao controlador!");

                if (model.Id != id)
                    throw new Exception("Identificador do registro difere do identificador do model");

                var entity = _mapper.Map<T>(model);

                var validationErrors = GenericValidator.Validate(entity);

                if (validationErrors.Any())
                    return new DataResult
                    {
                        Success = false,
                        Message = string.Join(Environment.NewLine, validationErrors)
                    };

                await _service.Update(entity);

                return new DataResult
                {
                    Success = true,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

        public virtual async Task<DataResult> Delete(long id)
        {
            try
            {
                await _service.Delete(id);

                return new DataResult
                {
                    Success = true,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

    }
}
