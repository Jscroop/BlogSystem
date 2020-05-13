using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Model;
using BlogSystem.Model.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.BLL
{
    /// <summary>
    /// 实现文章接口方法
    /// </summary>
    public class ArticleService : BaseService<Article>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleInCategoryRepository _articleInCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        //构造函数注入相关接口
        public ArticleService(IArticleRepository articleRepository, IArticleInCategoryRepository articleInCategoryRepository,
        ICategoryRepository categoryRepository)
        {
            _articleRepository = articleRepository;
            BaseRepository = articleRepository;
            _articleInCategoryRepository = articleInCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateArticleAsync(CreateArticleViewModel model)
        {
            //新增文章
            var article = new Article()
            {
                UserId = model.UserId,
                Title = model.Title,
                Content = model.Content
            };
            await _articleRepository.CreateAsync(article);

            //新增文章所属分类
            var articleId = article.Id;
            foreach (var categoryId in model.CategoryIds)
            {
                await _articleInCategoryRepository.CreateAsync(new ArticleInCategory()
                {
                    ArticleId = articleId,
                    CategoryId = categoryId
                }, false);
            }
            await _articleInCategoryRepository.SavedAsync();
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task EditArticleAsync(EditArticleViewModel model)
        {
            //保存文章更新
            var article = await _articleRepository.GetOneByIdAsync(model.Id);
            article.Title = model.Title;
            article.Content = model.Content;
            await _articleRepository.EditAsync(article);

            //删除所属分类
            var categoryIds = _articleInCategoryRepository.GetAll();
            foreach (var categoryId in categoryIds)
            {
                await _articleInCategoryRepository.RemoveAsync(categoryId, false);
            }
            //添加所属分类
            foreach (var categoryId in model.CategoryIds)
            {
                await _articleInCategoryRepository.CreateAsync(new ArticleInCategory()
                {
                    ArticleId = model.Id,
                    CategoryId = categoryId
                }, false);
            }
            //统一保存
            await _articleInCategoryRepository.SavedAsync();
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ArticleDetailsViewModel> GetArticleDetailsByArticleIdAsync(Guid articleId)
        {
            var data = await _articleRepository.GetAll().Include(m => m.User).Where(m => m.Id == articleId)
                .Select(m => new ArticleDetailsViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    Account = m.User.Account,
                    ProfilePhoto = m.User.ProfilePhoto,
                    GoodCount = m.GoodCount,
                    BadCount = m.BadCount
                }).FirstAsync();
            //处理分类
            var categories = await _articleInCategoryRepository.GetAll().Include(m => m.Category)
                .Where(m => m.ArticleId == data.Id).ToListAsync();
            data.CategoryIds = categories.Select(m => m.CategoryId).ToList();
            data.CategoryNames = categories.Select(m => m.Category.CategoryName).ToList();
            return data;
        }

        /// <summary>
        /// 根据用户Id获取文章列表信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ArticleListViewModel>> GetArticlesByUserIdAsync(Guid userId)
        {
            var list = await _articleRepository.GetAllByOrder(false).Include(m => m.User).Where(m => m.UserId == userId)
                .Select(m => new ArticleListViewModel()
                {
                    ArticleId = m.Id,
                    Title = m.Title,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    Account = m.User.Account,
                    ProfilePhoto = m.User.ProfilePhoto
                }).ToListAsync();
            return list;
        }

        /// <summary>
        /// 通过分类Id获取文章列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<List<ArticleListViewModel>> GetArticlesByCategoryIdAsync(Guid categoryId)
        {
            var data = await _categoryRepository.GetOneByIdAsync(categoryId);
            var userId = data.UserId;
            return await GetArticlesByUserIdAsync(userId);
        }

        /// <summary>
        /// 获取用户文章数量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<int> GetArticleCountByUserIdAsync(Guid userid)
        {
            return await _articleRepository.GetAll().CountAsync(m => m.UserId == userid);
        }

        /// <summary>
        /// 看好数量+1
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task AddGoodCount(Guid articleId)
        {
            var article = await _articleRepository.GetOneByIdAsync(articleId);
            article.GoodCount++;
            await _articleRepository.EditAsync(article);
        }

        /// <summary>
        /// 不看好数量+1
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task AddBadCount(Guid articleId)
        {
            var article = await _articleRepository.GetOneByIdAsync(articleId);
            article.BadCount--;
            await _articleRepository.EditAsync(article);
        }
    }
}
