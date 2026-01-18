using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkManagement.Common;
using WorkManagement.Models;
using WorkManagement.Services.AI;
using WorkManagement.Services.Clients;

namespace WorkManagement.Controllers.AI
{
    [Produces("application/json")]
    [Route("[controller]")]
    [PermissionDefinition("AiAssistant - AI", GroupName = "AI")]
    [ApiController]
    public class AiAssistantController : ControllerBase
    {
        public readonly IUserInfo userInfo;
        public readonly IAIAsissTantService aiAsissTantService;

        public AiAssistantController(IAIAsissTantService service, IUserInfo userInfo)
        {
            this.aiAsissTantService = service;
            this.userInfo = userInfo;
        }

        /// <summary>
        /// Chat với AI (text / voice → text)
        /// </summary>
        [HttpPost("chat")]
        [PermissionDefinition("Chat - Trò chuyện với AI",0,AllowAllAuthentcatedUser = true)]
        public async Task<IActionResult> Chat([FromBody] AiChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return BadRequest("Message is empty");

            // ClaimsPrincipal đã có sẵn trong Controller
            var result = await aiAsissTantService.HandleAsync(request.Message, User);

            return Ok(Result<string>.Success(result));
        }
    }
}
