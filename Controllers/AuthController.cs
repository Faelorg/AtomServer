using AtomServer.Models;
using AtomServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtomServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService service;
        public AuthController(IAuthService service)
        {
            this.service = service;
        }
        [HttpPost("reg")]
        public async Task<IActionResult> Register(UserRequestModel model)
        {
            var res = await service.Register(model);

            return StatusCode(res.code, res.result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequestModel model)
        {
            var res = await service.Login(model);

            return StatusCode(res.code, res.result);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var res = await service.Logout();

            return StatusCode(res.code, res.result);
        }


    }
}
