using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Models;

namespace WorkManagement.Services.Clients
{
    public interface IWmUsersService : IBaseService<Users>
    {
        Task<object> GetDetail(int id);
        Task<List<UserSearchDto>> SearchByName(string name);
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

            //return Result<object>.Success(data);
            return data;
        }
        public async Task<List<UserSearchDto>> SearchByName(string name)
        {
            return await Db.Users
                .Where(x => x.Name.Contains(name))
                .Select(x => new UserSearchDto
                {
                    UserIdGuid = x.UserIdGuid,
                    UserId = x.UserId,
                    Name = x.Name,
                    UserName = x.UserName,
                    Email = x.Email
                })
                .ToListAsync();
        }


    }
}
