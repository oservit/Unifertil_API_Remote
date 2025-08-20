using Application.Features.Products;
using Application.Features.Sync.Core;
using Application.Features.Sync.Products;
using Application.Services.Core;
using Libs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Central.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Product/Sync")]
    public class ProductSyncController : ControllerBase
    {
        private readonly IProductSyncAppService _syncService;

        public ProductSyncController(IProductSyncAppService syncService)
        {
            _syncService = syncService;
        }

        /// <summary>
        /// Endpoint de sincronização: insere ou atualiza produto
        /// </summary>
        [HttpPost("Receive")]
        public async Task<ActionResult<ApiResponse<DataResult>>> Sync([FromBody] ProductViewModel model)
        {
            var result = await _syncService.Sync(model);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
