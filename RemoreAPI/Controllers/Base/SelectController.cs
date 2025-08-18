using Microsoft.AspNetCore.Mvc;
using Libs.Base;
using Libs;
using Application.Features.Base;

namespace API.Remote.Controllers
{
    [Route("api/[controller]")]
    public class SelectController<T> : Controller where T : class
    {
        protected readonly IAppSelectService<T> _selectService;

        public SelectController(IAppSelectService<T> selectService)
        {
            _selectService = selectService;
        }

        [HttpGet("GetList")]
        public virtual async Task<ActionResult<DataResult>> GetList([FromQuery] string? filterJson = null)
        {

                var result = await _selectService.GetList(new FilterRequest(filterJson));
                if (result.Success)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [HttpGet("GetListPaged")]
        public virtual async Task<ActionResult<DataResult>> ListAllPaged(int pageIndex = 1, int pageSize = 20)
        {
                var result = await _selectService.GetListPaged(pageIndex, pageSize);
                if (result.Success)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, result);         
        }

        [HttpGet("data")]
        public virtual async Task<ActionResult<DataResult>> GetPagedResults(
            [FromQuery] string? filterJson = null,
            [FromQuery] string? orderByJson = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20)
        {

            var result = await _selectService.GetPagedResults(new FilterRequest(filterJson), new OrderByRequest(orderByJson), pageIndex, pageSize);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}