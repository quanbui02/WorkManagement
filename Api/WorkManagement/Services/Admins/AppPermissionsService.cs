using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using WorkManagement.Common;
using WorkManagement.Models;

namespace WorkManagement.Services.Admins
{
    public interface IAppPermissionsService
    {
        Task<object> GetPermissions();
        Task<object> GrantedRole(string roleId);
    }
    public class AppPermissionsService : IAppPermissionsService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserInfo _userInfo;
        private readonly AppDbContext _db;

        public AppPermissionsService(RoleManager<AppRole> roleManager, IConfiguration configuration, IUserInfo userInfo, AppDbContext db)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _userInfo = userInfo;
            _db = db;
        }

        public async Task<object> GetPermissions()
        {
            var data = await _db.AppController
                   .Include(x => x.AppPermission)
                   .OrderBy(x => x.Index)
                   .Select(x => new
                   {
                       x.Id,
                       x.Name,
                       x.DisplayName,
                       x.Route,
                       x.Index,
                       x.GroupName,
                       Permissions = x.AppPermission
                           .OrderBy(p => p.Index)
                           .Select(p => new
                           {
                               p.Id,
                               p.Name,
                               p.DisplayName,
                               p.PermissionCode,
                               p.Route,
                               p.Index,
                               p.Description
                           }).ToList()
                   })
                   .ToListAsync();

            return Result<object>.Success(data, data.Count, "Lấy danh sách quyền thành công.");
        }

        public async Task<object> GrantedRole(string roleId)
        {
            var granted = await _db.AppGrantedPermissions
                .Where(g => g.RoleId == roleId)
                .ToListAsync();

            if (!granted.Any())
                return Result<object>.Success(new { }, 1, "Không có quyền nào");

            var controllerIds = granted.Select(g => g.AppControllerId).Distinct().ToList();

            var controllers = await _db.AppController
                .Where(c => controllerIds.Contains(c.Id))
                .ToListAsync();

            var data = granted.Join(
                controllers,
                g => g.AppControllerId,
                c => c.Id,
                (g, c) => new
                {
                    ControllerName = c.Name,
                    g.PermissionValue
                }
            ).ToList();

            var result = data.ToDictionary(
                x => x.ControllerName,
                x => x.PermissionValue
            );

            return Result<object>.Success(result, 1, "Lấy quyền thành công");
        }


    }
}
