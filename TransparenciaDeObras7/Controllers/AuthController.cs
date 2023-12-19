using Microsoft.AspNetCore.Mvc;
using TransparenciaDeObras7.Services;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "username" && password == "123456")
            {
                var token = TokenService.GenerateToken(new Domain.User());
                return Ok(token);
            }

            return BadRequest("username or password invalid");
        }
    }
}