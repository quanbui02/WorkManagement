using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Services;
using WorkManagement.Services.Clients;

namespace WorkManagement.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [PermissionDefinition("Users - người dùng", GroupName = "Cấu hình người dùng")]
    [ApiController]
    public class UsersController : BaseController<Users, IUsersService>
    {
        private readonly IUsersService service;
    public readonly IUserInfo userInfo;

    public UsersController(IUsersService service, IUserInfo userInfo) : base(service)
    {
        this.service = service;
        this.userInfo = userInfo;
    }

        [HttpGet("getCurrent")]
        [PermissionDefinition("GetCurrent - Xem chi tiết", 0, AllowAllAuthentcatedUser = true)]
        public async Task<IActionResult> GetCurrent()
        {
            var re = await service.GetCurrentUser();
            return Ok(re);
        }
    }
}
