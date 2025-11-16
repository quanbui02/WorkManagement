using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkManagement.Common;
using WorkManagement.Services.Admins;

namespace WorkManagement.Controllers.Admins
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AppPermissionsController : ControllerBase
    {
        private readonly IAppPermissionsService _AppPermission;
        private readonly IUserInfo _userInfo;

        public AppPermissionsController(IAppPermissionsService AppPermission, IUserInfo userInfo)
        {
            _AppPermission = AppPermission;
            _userInfo = userInfo;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var data = await _AppPermission.GetPermissions();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpGet("Granted/Role/{roleId}")]
        public async Task<IActionResult> GetRoles(string roleId)
        {
            try
            {
                var data = await _AppPermission.GrantedRole(roleId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }
    }
}
