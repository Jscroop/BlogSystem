using System;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.Model;

namespace BlogSystem.IDAL
{
    /// <summary>
    /// 基类接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task CreateAsync(TEntity entity, bool saved = true);

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task RemoveAsync(Guid id, bool saved = true);

        /// <summary>
        /// 根据model删除对数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity, bool saved = true);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task EditAsync(TEntity entity, bool saved = true);

        /// <summary>
        /// 通过Id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetOneByIdAsync(Guid id);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 获取所有数据并排序
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAllByOrder(bool asc = true);

        /// <summary>
        /// 统一保存方法
        /// </summary>
        /// <returns></returns>
        Task SavedAsync();

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Exists(Guid id);
    }
}
