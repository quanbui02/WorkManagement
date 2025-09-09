using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkManagementNotification.Models;
using WorkManagementNotification.Repositories;

namespace WorkManagementNotification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationRepository _repo;

        public NotificationsController(NotificationRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification notification)
        {
            await _repo.CreateAsync(notification);
            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetForUser(string userId)
        {
            var result = await _repo.GetByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _repo.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
