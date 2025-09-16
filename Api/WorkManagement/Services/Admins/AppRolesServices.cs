using System.Net.WebSockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using Work.DataContext.Identity;
using WorkManagement.Common;
using WorkManagement.Models;

namespace WorkManagement.Services.Admins
{
    public interface IAppRolesServices
    {
        Task<IResult<AppRole>> Save(AppRole form);
        Task<object> AssignRolePermissions(AssignRolePermissions form);
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

            if (string.IsNullOrEmpty(form.RoleId) || form.PermissionIds == null || !form.PermissionIds.Any())
                return Result<object>.Error("Thiếu RoleId hoặc danh sách PermissionIds");

            var roleExists = await _db.Roles.AnyAsync(r => r.Id == form.RoleId);
            if (!roleExists)
                return Result<object>.Error("Không tìm thấy Role");

            var oldPermissions = _db.RolePermission.Where(rp => rp.RoleId == form.RoleId);
            _db.RolePermission.RemoveRange(oldPermissions);

            foreach (var permissionId in form.PermissionIds.Distinct())
            {
                _db.RolePermission.Add(new RolePermission
                {
                    RoleId = form.RoleId,
                    AppPermissionId = permissionId
                });
            }

            await _db.SaveChangesAsync();

            return Result<object>.Success(null, 1, "Gán quyền cho role thành công");
        }
    }
}
