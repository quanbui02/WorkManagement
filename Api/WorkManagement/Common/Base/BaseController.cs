using Microsoft.AspNetCore.Mvc;
using WorkManagement.Services;

namespace WorkManagement.Common
{
    public abstract class BaseController<TModel, TInterface> : Controller where TModel : class where TInterface : IBaseService<TModel>
    {
        public enum EnumPermission
        {
            List = 1,
            Detail,
            Update,
            Delete,
            DeleteRange
        }

        private readonly TInterface _service;

        public BaseController(TInterface service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        [PermissionDefinition("Xem chi tiết", 2, AllowAllAuthentcatedUser = true)]
        public virtual async Task<IActionResult> Get(int id)
        {
            TInterface service = _service;
            return Ok(Result<object>.Success(await service.GetAsync(id)));
        }

        [HttpDelete("{id:int}")]
        [PermissionDefinition("Xóa", 4)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            TInterface service = _service;
            TModel obj = service.Get(id);
            service = _service;
            service.Delete(obj);
            service = _service;
            await service.Save();
            return Ok(Result<object>.Success(obj, 0, "Xóa thành công"));
        }

        [HttpDelete]
        [PermissionDefinition("Xóa nhiều", 5)]
        public virtual async Task<IActionResult> DeleteRange(string ids)
        {
            List<int> listInt = MyBase.ConvertStringToListInt(ids);
            TInterface service;
            foreach (int item in listInt)
            {
                service = _service;
                TModel obj = service.Get(item);
                service = _service;
                service.Delete(obj);
            }

            service = _service;
            await service.Save();
            return Ok(Result<object>.Success(listInt, listInt.Count, "Xóa thành công"));
        }
    }
}
