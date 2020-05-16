using BlogSystem.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Model.ViewModels;

namespace BlogSystem.IBLL
{
    /// <summary>
    /// 分类服务接口
    /// </summary>
    public interface ICategoryService : IBaseService<Category>
    {
        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Guid> CreateCategory(string categoryName, Guid userId);

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> EditCategory(EditCategoryViewModel model, Guid userId);

        /// <summary>
        /// 通过用户Id获取所有分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CategoryListViewModel>> GetCategoryByUserIdAsync(Guid userId);
    }
}
