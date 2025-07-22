using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;

namespace WorkManagement.Services.Admins
{

    public interface IAuthService
    {
        Task<(string token, DateTime expires)> Login(string username, string password);
    }

    public class AuthServices : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly WorkManagementContext _context;

        public AuthServices(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IConfiguration configuration, WorkManagementContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<(string token, DateTime expires)> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new UnauthorizedAccessException("Tài khoản không tồn tại");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Sai mật khẩu");

            if (!user.IsSuperUser)
                throw new UnauthorizedAccessException("Tài khoản không có quyền truy cập admin");

            var roles = await _userManager.GetRolesAsync(user);
            var userContext = await _context.Users.FirstOrDefaultAsync(u => u.UserIdGuid == user.Id);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("username", userContext.UserName ?? ""),
                new Claim("avatar", userContext.Avatar ?? ""),
                new Claim("phone_number", user.PhoneNumber ?? ""),
                new Claim("email", user.Email ?? ""),
                new Claim("token", ""),
                new Claim("fullname", $"{userContext.Name}".Trim()),

                new Claim("issuperuser", user.IsSuperUser.ToString()),
                new Claim("userId", userContext.UserId.ToString()),
                new Claim("userguid", user.Id),
                new Claim("idClient", userContext.IdClient?.ToString() ?? "0"),
                new Claim("idShop", userContext.IdShop?.ToString() ?? "0"),
                new Claim("idType", userContext.IdType?.ToString() ?? "0"),
                new Claim("roleassign", JsonConvert.SerializeObject(roles)),
                new Claim("permissions", "{}"),
                //new Claim("listOrganization", string.Join(";", user.ListOrganization ?? new List<int>())),
                // optional
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("scope", "openid"),
                new Claim("scope", "profile"),
                new Claim("scope", "email"),
                new Claim("amr", "pwd")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expires);
        }
    }
}
