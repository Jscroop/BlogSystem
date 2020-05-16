using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.BLL
{
    /// <summary>
    /// 基类方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity, new()
    {
        //通过在子类的构造函数中注入，这里是基类，不用构造函数
        public IBaseRepository<TEntity> BaseRepository;

        /// <summary>
        /// 根据Id删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Guid id)
        {
            await BaseRepository.RemoveAsync(id);
        }

        /// <summary>
        /// 根据实体删除对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task RemoveAsync(TEntity entity)
        {
            await BaseRepository.RemoveAsync(entity);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetOneByIdAsync(Guid id)
        {
            return await BaseRepository.GetOneByIdAsync(id);
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await BaseRepository.GetAll().ToListAsync();
        }

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await BaseRepository.Exists(id);
        }
    }
}
