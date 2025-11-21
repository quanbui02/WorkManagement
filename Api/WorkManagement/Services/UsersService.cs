using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;

namespace WorkManagement.Services
{
    public interface IUsersService : IBaseService<Users>
    {
        Task<object> GetCurrentUser();
    }

    public class UsersService : BaseService<Users, WorkManagementContext>, IUsersService
    {
        private readonly IUserInfo _userInfo;
        public UsersService(WorkManagementContext db, ICachingHelper cachingHelper, IUserInfo user) : base(db, cachingHelper, user)
        {
            _userInfo = user;
        }

        public async Task<object> GetCurrentUser()
        {
            var user = await Db.Users.Where(s => s.UserId == _userInfo.UserId).Select(s => new
            {
                s.UserId,
                s.UserName,
                s.Name,
                s.Email,
                s.Phone,
                s.Avatar,
                s.UserIdGuid,
                s.IdWard,
                s.IdDistrict,
                s.IdProvince,
                s.IdType,
                s.IdParent,
                s.IsApproved,
                s.IdClient,
                s.IdShop,
                s.Address,
                s.IdBank,
                s.BankFullName,
                s.BankNumber,
                s.BankCardNumber,
                s.IdBankNavigation,
            }).FirstOrDefaultAsync();

            return Result<object>.Success(user);
        }
    }
}
