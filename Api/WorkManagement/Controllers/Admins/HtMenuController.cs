using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Services.Admins;

namespace WorkManagement.Controllers.Admins
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class HtMenuController : ControllerBase
    {
        private readonly IHtMenuServices _HtMenuServices;
        private readonly IUserInfo _userInfo;
        public HtMenuController(IHtMenuServices IHtMenuServices, IUserInfo userInfo)
        {
            _HtMenuServices = IHtMenuServices;
            _userInfo = userInfo;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] HtMenu form)
        {
            try
            {
                var data = await _HtMenuServices.Save(form);
                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Gets(int id)
        {
            try
            {
                var data = await _HtMenuServices.Gets(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var data = await _HtMenuServices.Delete(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }

        [HttpGet("GetDetail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var data = await _HtMenuServices.GetDetail(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
        }
    }
}
