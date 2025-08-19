using Application.Services.Clients;
using Application.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Remote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductRemoteController : ControllerBase
    {
        private readonly IProductRemoteAppService _productService;

        public ProductRemoteController(IProductRemoteAppService productService)
        {
            _productService = productService;
        }

        [HttpGet("ListAll")]
        public async Task<IActionResult> ListAll()
        {
            var clients = await _productService.ListAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var client = await _productService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }
    }
}
