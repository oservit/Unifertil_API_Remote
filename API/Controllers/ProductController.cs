using Application.Central.Features.Products;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Central.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _appService;
        private readonly IMapper _mapper;

        public ProductController(IProductAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        [HttpGet("ListAll")]
        public async Task<IActionResult> ListAll()
        {
            var products = await _appService.GetListPaged();
            return Ok(products);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var product = await _appService.Get(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
