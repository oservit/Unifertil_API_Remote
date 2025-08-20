using Application.Features.Products;
using Application.Features.Sync.Products;
using Application.Services.Core;
using Application.Services.Sync.Products;
using Libs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Central.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/Product/Sync")]
    public class ProductSyncController : ControllerBase
    {
        private readonly IProductSyncAppService _appService;
        private readonly IProductSyncRemoteService _remoteService;

        public ProductSyncController(
            IProductSyncAppService appService,
            IProductSyncRemoteService remoteService)
        {
            _appService = appService;
            _remoteService = remoteService;
        }

        /// <summary>
        /// Recebe um produto para sincronizar localmente
        /// </summary>
        [HttpPost("Receive")]
        public async Task<ActionResult<ApiResponse<DataResult>>> Receive([FromBody] ProductViewModel model)
        {
            var result = await _appService.SyncLocal(model);

            if (result.Success)
                return Ok(result);

            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        /// <summary>
        /// Envia um produto para sincronização na API Central
        /// </summary>
        [HttpPost("Send")]
        public async Task<ActionResult<ApiResponse<DataResult>>> Send([FromBody] ProductViewModel model)
        {
            var result = await _remoteService.SyncRemote(model);

            if (result.Success)
                return Ok(result);

            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
