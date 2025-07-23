using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext;
using WorkManagement.Common;
using WorkManagement.Services.Admins;

namespace WorkManagement.Controllers.Admins
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [PermissionDefinition("Permission Management - Đồng bộ quyền", GroupName = "Phân quyền")]
    public class SyncPermissionController : ControllerBase
    {
        private readonly ISyncPermissionServices _SyncPermissionServices;
        private readonly IUserInfo _userInfo;

        public SyncPermissionController(ISyncPermissionServices SyncPermissionServices, IUserInfo userInfo)
        {
            _SyncPermissionServices = SyncPermissionServices;
            _userInfo = userInfo;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncPermission()
        {
            try
            {
                var data = await _SyncPermissionServices.ScanAndSavePermissionsAsync();
                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Ok(new { message = ex.Message });
            }
        }
    }
}
