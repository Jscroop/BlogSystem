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
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateCategory(CreateCategoryViewModel model);

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditCategory(EditCategoryViewModel model);

        /// <summary>
        /// 通过用户Id获取所有分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CategoryListViewModel>> GetCategoryByUserIdAsync(Guid userId);
    }
}
