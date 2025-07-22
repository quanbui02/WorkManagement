using Microsoft.EntityFrameworkCore;
using WorkManagement.Common;

namespace WorkManagement.Services
{
    public abstract class BaseService<TModel, TDbContext> : IBaseService<TModel> where TModel : class where TDbContext : DbContext
    {
        public readonly TDbContext Db;

        public readonly ICachingHelper CachingHelper;

        public readonly IUserInfo User;

        public BaseService(TDbContext db, ICachingHelper cachingHelper, IUserInfo userInfo)
        {
            Db = db;
            CachingHelper = cachingHelper;
            User = userInfo;
        }

        public virtual TModel Get(object id)
        {
            return Db.Set<TModel>().Find(id);
        }

        public virtual async Task<TModel> GetAsync(object id)
        {
            return await Db.Set<TModel>().FindAsync(id);
        }

        public virtual IQueryable<TModel> GetAll()
        {
            return from c in Db.Set<TModel>()
                   select (c);
        }

        public virtual void Add(TModel obj)
        {
            Db.Set<TModel>().AddAsync(obj);
        }

        public virtual async Task Save()
        {
            await Db.SaveChangesAsync();
        }

        public virtual void Delete(TModel obj)
        {
            Db.Set<TModel>().Remove(obj);
        }
    }
}
