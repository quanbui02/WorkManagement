using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext;
using WorkManagement.Common;
using WorkManagement.Models;
using WorkManagement.Services.Admins;

namespace WorkManagement.Controllers.Admins
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServices _AccountServices;
        private readonly IUserInfo _userInfo;
        public AccountsController(IAccountServices AccountServices, IUserInfo userInfo)
        {
            _AccountServices = AccountServices;
            _userInfo = userInfo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserAsync form)
        {
            var data = await _AccountServices.CreateUserAsync(form);
            return Ok(data);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword form)
        {
            var data = await _AccountServices.ResetPassword(form);
            return Ok(data);
        }

        [HttpGet("Managerment")]
        public async Task<IActionResult> Managerment(string? key, int? userId, string? roleId, int? isSuperUser, int? isDisable, int? isActive, int offset = 0, int limit = 20)
        {
            var data = await _AccountServices.Managerment(key, userId, roleId, isSuperUser, isDisable, isActive, offset, limit);
            return Ok(data);
        }

        [HttpGet("GetByUserId/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var data = await _AccountServices.GetByUserId(id);
            return Ok(data);
        }
    }
}

