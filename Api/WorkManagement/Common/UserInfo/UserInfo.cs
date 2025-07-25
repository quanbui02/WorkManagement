using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WorkManagement.Common
{
    public class UserInfo : IUserInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                IsAuthenticated = true;

                UserId = int.Parse(user.FindFirst("userId")?.Value ?? "0");
                UserIdGuid = user.FindFirst("userguid")?.Value ?? "";
                IdClient = int.Parse(user.FindFirst("idClient")?.Value ?? "0");
                IdShop = int.Parse(user.FindFirst("idShop")?.Value ?? "0");
                IdPortal = 0;

                UserName = user.FindFirst("username")?.Value ?? "";
                DisplayName = user.FindFirst("fullname")?.Value ?? "";
                FullName = DisplayName;

                Avatar = user.FindFirst("avatar")?.Value ?? "";
                PhoneNumber = user.FindFirst("phone_number")?.Value ?? "";
                Email = user.FindFirst("email")?.Value ?? "";

                IsSuperUser = bool.TryParse(user.FindFirst("issuperuser")?.Value, out var super) && super;

                RoleAssign = user.FindFirst("roleassign")?.Value ?? "";
                IdType = int.Parse(user.FindFirst("idType")?.Value ?? "0");
                IsLeader = null;

                ListOrganization = new();
                ListRoleAssign = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            }
            else
            {
                IsAuthenticated = false;
            }

            Token = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "") ?? "";
        }

        public bool IsAuthenticated { get; }
        public int UserId { get; }
        public string UserIdGuid { get; }
        public int IdClient { get; }
        public int IdShop { get; }
        public int IdPortal { get; }
        public string UserName { get; }
        public string DisplayName { get; }
        public string Avatar { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string Token { get; }
        public string FullName { get; }
        public bool IsSuperUser { get; }
        public string RoleAssign { get; }
        public int IdType { get; }
        public List<int> ListOrganization { get; } = new();
        public List<string> ListRoleAssign { get; } = new();
        public bool? IsLeader { get; }
    }
}
