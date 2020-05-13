using BlogSystem.Model;
using System;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    /// <summary>
    /// 基类服务接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// 根据Id删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(Guid id);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// 通过Id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetOneByIdAsync(Guid id);

        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id);
    }
}
