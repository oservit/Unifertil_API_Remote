using Microsoft.AspNetCore.Mvc;
using Application.Base;
using AutoMapper;
using Domain.Base;

namespace API.Central.Controllers
{
    [Route("api/[controller]")]
    public class BaseController<TCreateModel, TUpdateModel, TEntity> : SelectController<TEntity>
        where TCreateModel : IViewModelCreate
        where TUpdateModel : IViewModelUpdate
        where TEntity : class, IEntityBase
    {
        protected readonly IAppServiceBase<TEntity> _appServiceBase;
        private readonly IMapper _mapper;

        public BaseController(IAppServiceBase<TEntity> appServiceBase, IMapper mapper)
            : base(appServiceBase)
        {
            _appServiceBase = appServiceBase;
            _mapper = mapper;
        }

        [HttpPost("Save")]
        public virtual async Task<IActionResult> SaveAsync([FromBody] TCreateModel createModel)
        {
            var result = await _appServiceBase.Save(createModel);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [HttpPut("Update/{id}")]
        public virtual async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] TUpdateModel updateModel)
        {
            var result = await _appServiceBase.Update(id, updateModel);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var result = await _appServiceBase.Delete(id);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}