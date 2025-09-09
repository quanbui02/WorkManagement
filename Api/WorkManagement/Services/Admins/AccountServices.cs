using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Models;

namespace WorkManagement.Services.Admins
{
    public interface IAccountServices
    {
        Task<object> CreateUserAsync(CreateUserAsync form);
        Task<object> ResetPassword(ResetPassword form);
        Task<object> Managerment(string? key, int? userId, string? roleId, int? isSuperUser, int? isDisable, int? isActive, int offset, int limit);
        Task<object> GetByUserId(int idUser);
    }
    public class AccountServices : IAccountServices
    {
        private readonly WorkManagementContext _context;
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserInfo _userInfo;

        public AccountServices(WorkManagementContext context, AppDbContext db, UserManager<AppUser> userManager, IUserInfo userInfo)
        {
            _context = context;
            _db = db;
            _userManager = userManager;
            _userInfo = userInfo;
        }

        public async Task<object> CreateUserAsync(CreateUserAsync form)
        {
            try
            {
                if (!_userInfo.IsSuperUser)
                    return Result<object>.Success(null, -1, "Bạn không có quyền tạo tài khoản");

                if (!Lib.IsValidEmail(form.Email))
                    return Result<object>.Success(null, -1, "Email không hợp lệ");

                if (!Lib.IsValidPhone(form.Phone))
                    return Result<object>.Success(null, -1, "Số điện thoại không hợp lệ");

                var existUser = await _userManager.Users
                                .FirstOrDefaultAsync(u => u.UserName == form.UserName
                                       || u.Email == form.Email
                                       || u.PhoneNumber == form.Phone);

                if (existUser != null)
                {
                    if (existUser.UserName == form.UserName)
                        return Result<object>.Success(null, -1, "Tên tài khoản đã tồn tại");

                    if (existUser.Email == form.Email)
                        return Result<object>.Success(null, -1, "Email đã được sử dụng");

                    if (existUser.PhoneNumber == form.Phone)
                        return Result<object>.Success(null, -1, "Số điện thoại đã được sử dụng");
                }

                var appUser = new AppUser
                {
                    UserName = form.UserName,
                    Email = form.Email,
                    PhoneNumber = form.Phone,
                    EmailConfirmed = true,
                    Created = DateTime.Now,
                    DisplayName = form.Name,
                    FirstName = form.Name,
                    LastName = form.Name,
                    CreatedBy = _userInfo.UserId,
                    Address = "",
                    Avatar = "",
                };

                var result = await _userManager.CreateAsync(appUser, form.Password);

                if (!result.Succeeded)
                {
                    return Result<object>.Success((false, string.Join("; ", result.Errors.Select(e => e.Description))));
                }

                var user = new Users
                {
                    UserIdGuid = appUser.Id,
                    UserName = form.UserName,
                    Name = form.Name,
                    Email = form.Email,
                    IdClient = 1,
                    IdType = 1,
                    IsApproveCmt = false,
                    Balance = 0,
                    TotalReward = 0,
                    IsTrial = false,
                    IsLogout = false,
                    IsDeleted = false,
                    IsApproved = true,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = _userInfo.UserId,
                    Address = "",
                    Avatar = "",
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Result<object>.Success(user, 1, "Tạo tài khoản thành công");
            }
            catch (Exception ex)
            {
                return Result<object>.Error("Lỗi: " + ex.Message);
            }
          
        }

        public async Task<object> ResetPassword(ResetPassword form)
        {
            try
            {
                if (form.PasswordNew != form.PasswordNew2)
                    return Result<object>.Success(null, -1, "Mật khẩu không trùng khớp, vui lòng nhập đúng mật khẩu để đổi.");
                
                var user = await _userManager.FindByNameAsync(form.UserName);
                if (user == null)
                    return Result<object>.Success(null, -1, "Không tìm thấy user");

                var removePwd = await _userManager.RemovePasswordAsync(user);
                if (!removePwd.Succeeded)
                    return Result<object>.Success(null, -1, "Không thể xoá mật khẩu cũ");

                var addPwd = await _userManager.AddPasswordAsync(user, form.PasswordNew);
                if (!addPwd.Succeeded)
                    return Result<object>.Success(null, -1, string.Join("; ", addPwd.Errors.Select(e => e.Description)));

                return Result<object>.Success(true, 1, "Đặt lại mật khẩu thành công");
            }
            catch (Exception ex)
            {
                return Result<object>.Error("Lỗi: " + ex.Message);
            }
        }

        public async Task<object> Managerment(string? key, int? userId, string? roleId, int? isSuperUser, int? isDisable, int? isActive, int offset, int limit)
        {
            var baseQuery = from u in _db.Users
                            where (isSuperUser < 0 || u.IsSuperUser == (isSuperUser == 1))
                               && (isDisable < 0 || u.IsDisable == (isDisable == 1))
                               && (isActive < 0 || u.IsActive == (isActive == 1))
                               && (userId < 0 || u.IdUser == userId)
                            select u;

            if (!string.IsNullOrEmpty(roleId))
            {
                baseQuery = from u in baseQuery
                            join ur in _db.UserRoles on u.Id equals ur.UserId
                            where ur.RoleId == roleId
                            select u;
            }

            if (!string.IsNullOrEmpty(key))
            {
                var k = key.ToLower();
                baseQuery = baseQuery.Where(x =>
                    EF.Functions.Like(x.UserName.ToLower(), $"%{k}%") ||
                    EF.Functions.Like(x.DisplayName.ToLower(), $"%{k}%") ||
                    EF.Functions.Like(x.Email.ToLower(), $"%{k}%") ||
                    EF.Functions.Like(x.PhoneNumber.ToLower(), $"%{k}%"));
            }

            var total = await baseQuery.CountAsync();

            var users = await baseQuery
                .OrderByDescending(x => x.IdUser)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();

            var roleMap = await (from ur in _db.UserRoles
                                 join r in _db.Roles on ur.RoleId equals r.Id
                                 where userIds.Contains(ur.UserId)
                                 select new { ur.UserId, RoleName = r.Name })
                                 .ToListAsync();

            var result = users.Select(u => new UserManagement
            {
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsSuperUser = u.IsSuperUser,
                IsDisable = u.IsDisable,
                IdUser = u.IdUser,
                RoleNames = roleMap.Where(r => r.UserId == u.Id).Select(r => r.RoleName).ToList()
            }).ToList();

            return Result<object>.Success(result, total, "Lấy danh sách user thành công");
        }

        public async Task<object> GetByUserId(int idUser)
        {
            var data = await _db.Users.Where(u => u.IdUser == idUser).FirstOrDefaultAsync();
            return Result<object>.Success(data);
        }
    }
}
