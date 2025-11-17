using Microsoft.AspNetCore.Identity;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Common;
using WorkManagement.Models;

namespace WorkManagement.Services.Admins
{
    public interface IHtMenuServices
    {
        Task<IResult<HtMenu>> Save(HtMenu form);
        Task<object> Gets(int id);
        Task<object> Delete(int id);
        Task<object> GetDetail(int id);
    }
    public class HtMenuServices : IHtMenuServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInfo _userInfo;
        private readonly WorkManagementContext _db;

        public HtMenuServices(IConfiguration configuration, IUserInfo userInfo, WorkManagementContext db)
        {
            _configuration = configuration;
            _userInfo = userInfo;
            _db = db;
        }

        public async Task<IResult<HtMenu>> Save(HtMenu form)
        {
            HtMenu obj;
            if(form.Id > 0)
            {
                obj = await _db.HtMenu.FindAsync(form.Id);
                if(obj == null)
                {
                    return Result<HtMenu>.Error("Không tìm thấy bản ghi này!!!");
                }
            }
            else
            {
                obj = new HtMenu
                {
                    Created = DateTime.Now,
                    CreatedBy = _userInfo.UserId,
                    IsDeleted = false
                };

                _db.HtMenu.Add(obj);
            }

            obj.IdCha = form.IdCha;
            obj.Ma = form.Ma;
            obj.Ten = form.Ten;
            obj.Url = form.Url;
            obj.Icon = form.Icon;
            obj.ThuTu = form.ThuTu;
            obj.TenDuongDan = form.TenDuongDan;
            obj.IdDuongDan = form.IdDuongDan;
            obj.PhanQuyen = form.PhanQuyen;
            obj.PhanHe = form.PhanHe;
            obj.TrangThai = form.TrangThai;

            await _db.SaveChangesAsync();
            return Result<HtMenu>.Success(obj, 1, "Cập nhật mới thành công.");
        }
    
        public async Task<object> Gets(int id)
        {
            var query = from m in _db.HtMenu
                        where (id < 0 || m.Id == id)
                            && m.IsDeleted == false
                            && m.TrangThai == 1
                        select m;

            return Result<object>.Success(query);
        }
    
        public async Task<object> Delete(int id)
        {
            var crrMenu = await _db.HtMenu.FindAsync(id);
            crrMenu.IsDeleted = true;

            return Result<object>.Success(-1,0,"Xoá thành công.");
        }
    
        public async Task<object> GetDetail(int id)
        {
            var data = await _db.HtMenu.FindAsync(id);
            return Result<object>.Success(data);
        }
    }
}
