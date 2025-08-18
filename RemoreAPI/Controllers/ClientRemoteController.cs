using Application.Services.Clients;
using Microsoft.AspNetCore.Mvc;

namespace API.Remote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientRemoteController : ControllerBase
    {
        private readonly IClientRemoteAppService _clientService;

        public ClientRemoteController(IClientRemoteAppService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("ListAll")]
        public async Task<IActionResult> ListAll()
        {
            var clients = await _clientService.ListAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }
    }
}
