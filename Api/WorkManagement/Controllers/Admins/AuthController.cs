using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkManagement.Common;
using WorkManagement.Models;
using WorkManagement.Services.Admins;

namespace WorkManagement.Controllers.Admins
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _AuthService;
        public AuthController(IAuthService AuthService)
        {
            _AuthService = AuthService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var (token, expires) = await _AuthService.Login(model.Username, model.Password);
                return Ok(Result<object>.Success(new { token, expires }));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
