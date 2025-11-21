using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;

namespace WorkManagement.Services.Clients
{
    public interface IWmUsersService : IBaseService<Users>
    {
        Task<object> GetDetail(int id);
    }

    public class WmUsersService : BaseService<Users, WorkManagementContext>, IWmUsersService
    {
        private readonly IUserInfo _userInfo;
        public WmUsersService(WorkManagementContext db, ICachingHelper cachingHelper, IUserInfo user) : base(db, cachingHelper, user)
        {
            _userInfo = user;
        }

        public async Task<object> GetDetail(int id)
        {
            var query = from s in Db.Users
                        where s.UserId == id
                        select new
                        {
                            s.UserId,
                            s.UserName,
                            s.Avatar,
                            s.Name,
                            s.Phone,
                            s.Email
                        };
            var data = await query.FirstOrDefaultAsync();
            if (data is null)
            {
                return Result<object>.Error("Không tìm thấy dữ liệu này");
            }

            return Result<object>.Success(data);
        }
    }
}
