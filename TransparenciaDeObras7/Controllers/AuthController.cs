using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Configuration;
using TransparenciaDeObras7.Services;

namespace TransparenciaDeObras7.Controllers
{

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly AuthSettings _authSettings;

        public AuthController(IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == _authSettings.Username && password == _authSettings.Password)
            {
                var token = TokenService.GenerateToken(new Domain.User());
                return Ok(token);
            }

            return BadRequest("Username or password invalid");
        }
    }
}