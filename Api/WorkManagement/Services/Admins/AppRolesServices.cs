using System.Net.WebSockets;
using Microsoft.AspNetCore.Identity;
using Work.DataContext;
using WorkManagement.Common;

namespace WorkManagement.Services.Admins
{
    public interface IAppRolesServices
    {
        Task<IResult<AppRole>> Save(AppRole form);
    }
    public class AppRolesServices : IAppRolesServices
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserInfo _userInfo;

        public AppRolesServices(RoleManager<AppRole> roleManager, IConfiguration configuration, IUserInfo userInfo)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _userInfo = userInfo;
        }

        public async Task<IResult<AppRole>> Save(AppRole form)
        {
            AppRole obj;

            var userId = _userInfo.UserId;
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

    }
}
