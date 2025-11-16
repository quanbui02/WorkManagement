using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppRolesController : ControllerBase
    {
        private readonly IAppRolesServices _AppRolesServices;
        private readonly IUserInfo _userInfo;
        public AppRolesController(IAppRolesServices AppRolesServices, IUserInfo userInfo)
        {
            _AppRolesServices = AppRolesServices;
            _userInfo = userInfo;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AppRole form)
        {
            try
            {
                var data = await _AppRolesServices.Save(form);
                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRolePermissions form)
        {
            try
            {
                var data = await _AppRolesServices.AssignRolePermissions(form);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(string key, int offset, int limit)
        {
            try
            {
                var data = await _AppRolesServices.GetRoles(key, offset, limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpGet("GetDetail/{id}")]
        public async Task<IActionResult> GetDetail(string id)
        {
            try
            {
                var data = await _AppRolesServices.GetDetail(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            try
            {
                var data = await _AppRolesServices.Remove(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }
    }
}
