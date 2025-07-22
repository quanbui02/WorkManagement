using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext;
using WorkManagement.Common;
using WorkManagement.Services.Admins;

namespace WorkManagement.Controllers.Admins
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
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
    }
}
