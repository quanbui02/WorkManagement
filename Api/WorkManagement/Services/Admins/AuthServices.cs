using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Helper;
using WorkManagement.Common;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly AppDbContext _db;
        private readonly SecurityKey _rsaKey;
        private readonly ICachingHelper _cache;

        public AuthServices(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IConfiguration configuration, WorkManagementContext context, AppDbContext db, SecurityKey rsaKey, ICachingHelper cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _db = db;
            _rsaKey = rsaKey;
            _cache = cache;
        }

        public async Task<(string token, DateTime expires)> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new UnauthorizedAccessException("Tài khoản không tồn tại");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Sai mật khẩu");

            //if (!user.IsSuperUser)
            //    throw new UnauthorizedAccessException("Tài khoản không có quyền truy cập admin");

            var roles = await _userManager.GetRolesAsync(user);
            var userContext = await _context.Users.FirstOrDefaultAsync(u => u.UserIdGuid == user.Id);

            #region Lấy Role của user
            //var permissions = await (
            //    from ur in _db.UserRoles
            //    join rp in _db.RolePermission on ur.RoleId equals rp.RoleId
            //    join ap in _db.AppPermission on rp.AppPermissionId equals ap.Id
            //    join ac in _db.AppController on ap.AppControllerId equals ac.Id
            //    where ur.UserId == user.Id
            //    select new
            //    {
            //        Service = "workmanagement",
            //        Controller = ap.Controller,
            //        Index = ap.Index
            //    }
            //).ToListAsync();

            //var permissionDict = new Dictionary<string, Dictionary<string, long>>();

            //foreach (var p in permissions)
            //{
            //    if (!permissionDict.ContainsKey(p.Service))
            //        permissionDict[p.Service] = new Dictionary<string, long>();

            //    if (!permissionDict[p.Service].ContainsKey(p.Controller))
            //        permissionDict[p.Service][p.Controller] = 0;

            //    permissionDict[p.Service][p.Controller] |= (1L << p.Index.Value);
            //}
            #endregion

            var permissionDict = await GetPermissionsByUserAsync(user.Id);

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
                new Claim("permissions", JsonConvert.SerializeObject(permissionDict)),
                //new Claim("listOrganization", string.Join(";", user.ListOrganization ?? new List<int>())),
                // optional
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("scope", "openid"),
                new Claim("scope", "profile"),
                new Claim("scope", "email"),
                new Claim("amr", "pwd")
            };

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["Jwt:PrivateKeyPath"]);
            //var rsaKey = JwtKeyHelper.LoadPrivateKey(privateKeyPath);

            var creds = new SigningCredentials(_rsaKey, SecurityAlgorithms.RsaSha256);

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

        private async Task<Dictionary<string, Dictionary<string, long>>> GetPermissionsByUserAsync(string userId)
        {
            string cacheKey = $"permissions:{userId}";

            if (_cache.IsSet(cacheKey))
                return _cache.Get<Dictionary<string, Dictionary<string, long>>>(cacheKey);

            var userRoles = await _db.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();

            var mergedPermission = new Dictionary<string, Dictionary<string, long>>();

            foreach (var roleId in userRoles)
            {
                string roleCacheKey = $"permissions:role:{roleId}";

                Dictionary<string, Dictionary<string, long>> rolePerm = null;

                if (_cache.IsSet(roleCacheKey))
                {
                    rolePerm = _cache.Get<Dictionary<string, Dictionary<string, long>>>(roleCacheKey);
                }
                else
                {
                    var rolePermissions = await (
                        from rp in _db.RolePermission
                        join ap in _db.AppPermission on rp.AppPermissionId equals ap.Id
                        join ac in _db.AppController on ap.AppControllerId equals ac.Id
                        where rp.RoleId == roleId
                        select new { ap.Controller, ap.Index }
                    ).ToListAsync();

                    var roleDict = new Dictionary<string, Dictionary<string, long>>
                    {
                        ["workmanagement"] = new Dictionary<string, long>()
                    };

                    foreach (var p in rolePermissions)
                    {
                        if (!roleDict["workmanagement"].ContainsKey(p.Controller))
                            roleDict["workmanagement"][p.Controller] = 0;

                        if (p.Index.HasValue)
                            roleDict["workmanagement"][p.Controller] |= (1L << p.Index.Value);
                    }

                    // Cache quyền theo role
                    _cache.Set(roleCacheKey, roleDict, TimeSpan.FromHours(2));
                    rolePerm = roleDict;
                }

                MergePermission(mergedPermission, rolePerm);
            }

            // Cache quyền tổng hợp theo user
            _cache.Set(cacheKey, mergedPermission, TimeSpan.FromMinutes(30));

            return mergedPermission;
        }

        private void MergePermission(Dictionary<string, Dictionary<string, long>> target,Dictionary<string, Dictionary<string, long>> source)
        {
            foreach (var service in source)
            {
                if (!target.ContainsKey(service.Key))
                    target[service.Key] = new Dictionary<string, long>();

                foreach (var ctrl in service.Value)
                {
                    if (!target[service.Key].ContainsKey(ctrl.Key))
                        target[service.Key][ctrl.Key] = 0;

                    target[service.Key][ctrl.Key] |= ctrl.Value;
                }
            }
        }
    }
}
