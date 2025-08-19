using Libs;
using Libs.Common;
using Libs.Exceptions;
using Service.Common;

namespace Application.Common
{
    public class AppSelectService<T> : IAppSelectService<T> where T : class
    {
        protected readonly ISelectService<T> _service;
        public AppSelectService(ISelectService<T> service)
        {
            _service = service;
        }

        public virtual async Task<DataResult> Get(string id)
        {
            try
            {
                var entity = await _service.Get(id);
                return new DataResult
                {
                    Data = DynamicConverter.ConvertToExpandoObject(entity),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

        public virtual async Task<DataResult> Get(long id)
        {
            try
            {
                var entity = await _service.Get(id);
                return new DataResult
                {
                    Data = DynamicConverter.ConvertToExpandoObject(entity),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

        public virtual async Task<DataResult> GetList(FilterRequest? filter = null)
        {
            try
            {
                var entities = await _service.GetList(filter);
                return new DataResult
                {
                    Data = entities.Select(item => DynamicConverter.ConvertToExpandoObject(item)).ToList().Distinct(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreateErrorResult(ex);
            }
        }

        public virtual async Task<DataPagedResult> GetListPaged(int pageIndex, int pageSize)
        {
            try
            {
                var entities = await _service.GetList(pageIndex, pageSize);
                return new DataPagedResult
                {
                    Data = entities.Select(item => DynamicConverter.ConvertToExpandoObject(item)).ToList().Distinct(),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = await _service.Count(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreatePagedErrorResult(ex);
            }
        }

        public virtual async Task<DataPagedResult> GetPagedResults(FilterRequest filter, OrderByRequest order, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var entities = await _service.GetPagedResults(filter, order, pageIndex, pageSize);

                return new DataPagedResult
                {
                    Data = entities.Select(item => DynamicConverter.ConvertToExpandoObject(item)).ToList().Distinct(),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = await _service.Count(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return ExceptionHelper.CreatePagedErrorResult(ex);
            }
        }
    }
}