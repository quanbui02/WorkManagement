using System.Data;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work.API.Common;
using Work.DataContext;
using Work.DataContext.Identity;
using Work.DataContext.Migrations;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Models;

namespace WorkManagement.Services.Admins
{
    public interface IAppRolesServices
    {
        Task<IResult<AppRole>> Save(AppRole form);
        Task<object> AssignRolePermissions(AssignRolePermissions form);
        Task<object> GetRoles(string key, int offset, int limit);
        Task<object> Remove(string id);
        Task<object> GetDetail(string id);
    }
    public class AppRolesServices : IAppRolesServices
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserInfo _userInfo;
        private readonly AppDbContext _db;

        public AppRolesServices(RoleManager<AppRole> roleManager, IConfiguration configuration, IUserInfo userInfo, AppDbContext db)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _userInfo = userInfo;
            _db = db;
        }

        public async Task<IResult<AppRole>> Save(AppRole form)
        {
            AppRole obj;
            if (!string.IsNullOrEmpty(form.Id))
            {
                obj = await _roleManager.FindByIdAsync(form.Id);

                if (obj == null) return Result<AppRole>.Success(null, -1, "Role không tồn tại.");

                obj.Name = form.Name ?? obj.Name;
                obj.NormalizedName = form.Name?.ToUpper() ?? obj.NormalizedName;
                obj.Code = form.Code ?? obj.Code;
                obj.IsActive = form.IsActive;
                obj.UpdatedDate = DateTime.Now;
            }
            else
            {
                obj = new AppRole
                {
                    Name = form.Name,
                    NormalizedName = form.Name.ToUpper(),
                    Code = form.Code,
                    AutoAssign = form.AutoAssign,
                    IsActive = form.IsActive,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                var result = await _roleManager.CreateAsync(obj);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return Result<AppRole>.Error("Lỗi:" + errors);
                }

                return Result<AppRole>.Success(obj, 1, "Cập nhật Role thành công.");
            }

            var updateResult = await _roleManager.UpdateAsync(obj);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                return Result<AppRole>.Error("Lỗi:" + errors);
            }

            return Result<AppRole>.Success(obj, 1, "Cập nhật Role thành công.");
        }

        public async Task<object> AssignRolePermissions(AssignRolePermissions form)
        {
            if (string.IsNullOrEmpty(form.RoleId))
                return Result<object>.Error("Thiếu RoleId");

            if (form.PermissionIds == null)
                return Result<object>.Error("Danh sách PermissionIds bị null");

            var roleExists = await _db.Roles.AnyAsync(r => r.Id == form.RoleId);
            if (!roleExists)
                return Result<object>.Error("Không tìm thấy Role");

            var oldRolePermissions = _db.RolePermission.Where(rp => rp.RoleId == form.RoleId);
            _db.RolePermission.RemoveRange(oldRolePermissions);

            foreach (var permissionId in form.PermissionIds.Distinct())
            {
                _db.RolePermission.Add(new RolePermission
                {
                    RoleId = form.RoleId,
                    AppPermissionId = permissionId
                });
            }

            var oldGranted = _db.AppGrantedPermissions.Where(x => x.RoleId == form.RoleId);
            _db.AppGrantedPermissions.RemoveRange(oldGranted);

            var permissions = await _db.AppPermission
                .Where(p => form.PermissionIds.Contains(p.Id))
                .ToListAsync();

            var grouped = permissions.GroupBy(p => p.AppControllerId);

            foreach (var group in grouped)
            {
                int mask = 0;

                foreach (var p in group)
                {
                    mask |= (int)(1 << p.Index); // bitmask từ index
                }

                _db.AppGrantedPermissions.Add(new AppGrantedPermissions
                {
                    RoleId = form.RoleId,
                    AppControllerId = group.Key,
                    PermissionValue = mask,
                    CreatedUserId = _userInfo.UserId,
                    LastModifyUserId = _userInfo.UserId,
                    LastModifyDate = DateTime.Now
                });
            }

            await _db.SaveChangesAsync();

            return Result<object>.Success(null, 1, "Gán quyền cho role thành công");
        }

        public async Task<object> GetRoles(string key, int offset, int limit)
        {
            var roles = from s in _db.Roles
                        where s.IsDeleted != true && s.IsActive == true
                        select new
                        {
                            s.Id,
                            s.Name,
                            s.Code,
                            s.IsActive,
                            s.IsDeleted
                        };

            if (!string.IsNullOrEmpty(key))
            {
                roles = roles.Where(x => x.Name.ToLower().Contains(key.ToLower()));
            }

            var re = await roles.Skip(offset).Take(limit).ToListAsync();
            return Result<object>.Success(re, await roles.CountAsync());

        }
    
        public async Task<object> Remove(string id)
        {
            var role = await _db.Roles.Where(r => r.Id == id).FirstOrDefaultAsync();

            role.IsDeleted = true;
            role.IsActive = false;

            await _db.SaveChangesAsync();
            return Result<object>.Success(-1, -1, "Xoá thành công!");
        }

        public async Task<object> GetDetail(string id)
        {
            var crrRole = await _db.Roles.Where(s => s.Id == id).Select(s => new
            {
                s.Id,
                s.Name,
                s.Code,
            }).FirstOrDefaultAsync();

            return Result<object>.Success(crrRole, 1);
        }
    }
}
