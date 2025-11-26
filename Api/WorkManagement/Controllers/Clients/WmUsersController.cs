using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Services.Clients;

namespace WorkManagement.Controllers.Clients
{
    [Produces("application/json")]
    [Route("[controller]")]
    [PermissionDefinition("WmUsers - người dùng", GroupName = "Quản lý người dùng")]
    [ApiController]
    public class WmUsersController : BaseController<Users, IWmUsersService>
    {
        private readonly IWmUsersService service;
        public readonly IUserInfo userInfo;

        public WmUsersController(IWmUsersService service, IUserInfo userInfo) : base(service)
        {
            this.service = service;
            this.userInfo = userInfo;
        }

        [HttpGet("GetDetail")]
        [PermissionDefinition("GetDetail - Xem chi tiết", 0, AllowAllAuthentcatedUser = true)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var re = await service.GetDetail(id);
            return Ok(re);
        }

        [HttpGet("TestSynPermisstion")]
        [PermissionDefinition("TestSynPermisstion - Cập Nhật Method trong Controller", 6)]
        public async Task<IActionResult> TestSynPermisstion()
        {
            return Ok("Ok");
        }

        [HttpGet("TestSynPermisstion2")]
        [PermissionDefinition("TestSynPermisstion2 - Cập Nhật Method trong Controller", 7)]
        public async Task<IActionResult> TestSynPermisstion2()
        {
            return Ok("Ok2");
        }
    }
}
