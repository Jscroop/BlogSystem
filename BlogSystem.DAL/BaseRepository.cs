using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    /// <summary>
    /// 实现基类接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly BlogSystemContext _context;

        public BaseRepository(BlogSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task CreateAsync(TEntity entity, bool saved = true)
        {
            _context.Set<TEntity>().Add(entity);
            if (saved) await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Guid id, bool saved = true)
        {
            var t = new TEntity() { Id = id };
            _context.Entry(t).State = EntityState.Unchanged;
            t.IsRemoved = true;
            if (saved) await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据model删除数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task RemoveAsync(TEntity model, bool saved = true)
        {
            await RemoveAsync(model.Id, saved);
        }

        /// <summary>
        ///  修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task EditAsync(TEntity entity, bool saved = true)
        {
            _context.Entry(entity).State = EntityState.Modified;

            //如果数据没有发生变化
            if (!_context.ChangeTracker.HasChanges()) return;

            if (saved) await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 通过Id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetOneByIdAsync(Guid id)
        {
            return await GetAll().FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().Where(m => !m.IsRemoved).AsNoTracking();
        }

        /// <summary>
        /// 获取所有数据并排序
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllByOrder(bool asc = true)
        {
            var data = GetAll();
            data = asc ? data.OrderBy(m => m.CreateTime) : data.OrderByDescending(m => m.CreateTime);
            return data;
        }

        /// <summary>
        /// 统一保存方法
        /// </summary>
        /// <returns></returns>
        public async Task SavedAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 确认对象是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Exists(Guid id)
        {
            return await GetAll().AnyAsync(m => m.Id == id && m.IsRemoved == false);
        }
    }
}
