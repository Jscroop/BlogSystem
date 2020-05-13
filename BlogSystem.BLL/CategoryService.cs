using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Model;
using BlogSystem.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            BaseRepository = categoryRepository;
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateCategory(CreateCategoryViewModel model)
        {
            await _categoryRepository.CreateAsync(new Category()
            {
                UserId = model.UserId,
                CategoryName = model.CategoryName
            });
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task EditCategory(EditCategoryViewModel model)
        {
            await _categoryRepository.EditAsync(new Category()
            {
                UserId = model.UserId,
                Id = model.CategoryId,
                CategoryName = model.CategoryName
            });
        }

        /// <summary>
        ///  通过用户Id获取所有分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<CategoryListViewModel>> GetCategoryByUserIdAsync(Guid userId)
        {
            return _categoryRepository.GetAll().Where(m => m.UserId == userId).Select(m => new CategoryListViewModel
            {
                CategoryId = m.Id,
                CategoryName = m.CategoryName
            }).ToListAsync();
        }
    }
}
