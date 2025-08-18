using Microsoft.AspNetCore.Mvc;
using Libs.Base;
using Application.Features.Authentication;

namespace API.Remote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller    
    {
        private readonly IAuthenticationAppService _appService;

        public AuthController(IAuthenticationAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] UserViewModel authenticationUserModel)
        {
            DataResult result = await _appService.GetToken(authenticationUserModel);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
