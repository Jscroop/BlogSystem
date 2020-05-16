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
        /// <param name="categoryName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateCategory(string categoryName, Guid userId)
        {
            //当前用户存在该分类名称则返回
            if (string.IsNullOrEmpty(categoryName) || await _categoryRepository.GetAll()
                .AnyAsync(m => m.UserId == userId && m.CategoryName == categoryName))
            {
                return Guid.Empty;
            }
            //创建成功返回分类Id
            var categoryId = Guid.NewGuid();
            await _categoryRepository.CreateAsync(new Category
            {
                Id = categoryId,
                UserId = userId,
                CategoryName = categoryName
            });
            return categoryId;
        }

        /// <summary>
        ///  编辑分类
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> EditCategory(EditCategoryViewModel model, Guid userId)
        {
            //用户不存在该分类则返回
            if (!await _categoryRepository.GetAll().AnyAsync(m => m.UserId == userId && m.Id == model.CategoryId))
            {
                return false;
            }

            await _categoryRepository.EditAsync(new Category
            {
                UserId = userId,
                Id = model.CategoryId,
                CategoryName = model.CategoryName
            });
            return true;
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
