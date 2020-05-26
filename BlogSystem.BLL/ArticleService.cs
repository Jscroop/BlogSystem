using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Model;
using BlogSystem.Model.Parameters;
using BlogSystem.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.Common.Helpers.SortHelper;
using BlogSystem.Model.Helpers;

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
        private readonly IUserRepository _userRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        //构造函数注入相关接口
        public ArticleService(IArticleRepository articleRepository, IArticleInCategoryRepository articleInCategoryRepository,
        ICategoryRepository categoryRepository, IUserRepository userRepository, IPropertyMappingService propertyMappingService)
        {
            _articleRepository = articleRepository;
            BaseRepository = articleRepository;
            _articleInCategoryRepository = articleInCategoryRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _propertyMappingService = propertyMappingService;
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateArticleAsync(CreateArticleViewModel model, Guid userId)
        {
            //新增文章
            var articleId = Guid.NewGuid();
            var article = new Article
            {
                Id = articleId,
                UserId = userId,
                Title = model.Title,
                Content = model.Content
            };
            await _articleRepository.CreateAsync(article);

            //新增文章所属分类
            await CreateArticleInCategory(article.Id, model.CategoryIds);

            return articleId;
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> EditArticleAsync(EditArticleViewModel model, Guid userId)
        {
            //删除文章所属分类
            if (!await RemoveArticleInCategory(model.Id, userId))
            {
                return false;
            }

            //新增文章所属分类
            await CreateArticleInCategory(model.Id, model.CategoryIds);

            //保存文章更新
            var article = await _articleRepository.GetOneByIdAsync(model.Id);
            article.Title = model.Title;
            article.Content = model.Content;
            await _articleRepository.EditAsync(article);

            return true;
        }

        /// <summary>
        /// 通过文章Id获取文章详情
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ArticleDetailsViewModel> GetArticleDetailsByArticleIdAsync(Guid articleId)
        {
            if (!await _articleRepository.GetAll().AnyAsync(m => m.Id == articleId))
            {
                return new ArticleDetailsViewModel();
            }
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
            if (!await _articleRepository.GetAll().AnyAsync(m => m.UserId == userId))
            {
                return new List<ArticleListViewModel>();
            }
            return await _articleRepository.GetAllByOrder(false).Include(m => m.User)
                .Where(m => m.UserId == userId).Select(m => new ArticleListViewModel
                {
                    ArticleId = m.Id,
                    Title = m.Title,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    Account = m.User.Account,
                    ProfilePhoto = m.User.ProfilePhoto
                }).ToListAsync();
        }

        /// <summary>
        /// 通过用户分类Id获取文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<List<ArticleListViewModel>> GetArticlesByCategoryIdAsync(Guid userId, Guid categoryId)
        {
            //判断有无用户及分类信息
            if (!await _categoryRepository.GetAll().AnyAsync(m => m.UserId == userId && m.Id == categoryId))
            {
                return new List<ArticleListViewModel>();
            }

            var user = await _userRepository.GetOneByIdAsync(userId);

            return await _articleInCategoryRepository.GetAll().Include(m => m.Article)
                .Where(m => m.CategoryId == categoryId).Select(m => new ArticleListViewModel
                {
                    ArticleId = m.Id,
                    Title = m.Article.Title,
                    Content = m.Article.Content,
                    CreateTime = m.CreateTime,
                    Account = user.Account,
                    ProfilePhoto = user.ProfilePhoto
                }).ToListAsync();
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
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddGoodCount(Guid articleId, Guid userId)
        {
            var article = await _articleRepository.GetOneByIdAsync(articleId);
            article.GoodCount++;
            await _articleRepository.EditAsync(article);
        }

        /// <summary>
        /// 不看好数量+1
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddBadCount(Guid articleId, Guid userId)
        {
            var article = await _articleRepository.GetOneByIdAsync(articleId);
            article.BadCount++;
            await _articleRepository.EditAsync(article);
        }

        /// <summary>
        ///  删除文章所属分类信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveArticleInCategory(Guid articleId, Guid userId)
        {
            //判断是否为该用户文章
            if (!await _articleRepository.GetAll().AnyAsync(m => m.UserId == userId && m.Id == articleId))
            {
                return false;
            }
            //删除文章所属分类信息
            var categoryIds = _articleInCategoryRepository.GetAll().Where(m => m.ArticleId == articleId);
            foreach (var categoryId in categoryIds)
            {
                await _articleInCategoryRepository.RemoveAsync(categoryId, false);
            }
            await _articleInCategoryRepository.SavedAsync();
            return true;
        }

        /// <summary>
        /// 新增文章所属分类信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task CreateArticleInCategory(Guid articleId, List<Guid> categoryIds)
        {
            foreach (var categoryId in categoryIds)
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
        /// 文章过滤及搜索
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<PageList<ArticleListViewModel>> GetArticles(ArticleParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var resultList = _articleRepository.GetAll();

            var dateTime = DateTime.Now;

            //过滤条件，判断枚举是否引用
            if (Enum.IsDefined(typeof(DistanceTime), parameters.DistanceTime))
            {
                switch (parameters.DistanceTime)
                {
                    case DistanceTime.Week:
                        dateTime = dateTime.AddDays(-7);
                        break;
                    case DistanceTime.Month:
                        dateTime = dateTime.AddMonths(-1);
                        break;
                    case DistanceTime.Year:
                        dateTime = dateTime.AddYears(-1);
                        break;
                }
                resultList = resultList.Where(m => m.CreateTime > dateTime);
            }

            //搜索条件，暂时添加标题和内容
            if (!string.IsNullOrWhiteSpace(parameters.SearchStr))
            {
                parameters.SearchStr = parameters.SearchStr.Trim();
                resultList = resultList.Where(m =>
                    m.Title.Contains(parameters.SearchStr) || m.Content.Contains(parameters.SearchStr));
            }

            //转换为viewModel
            var list = resultList.Select(m => new ArticleListViewModel
            {
                ArticleId = m.Id,
                Title = m.Title,
                Content = m.Content,
                CreateTime = m.CreateTime,
                Account = m.User.Account,
                ProfilePhoto = m.User.ProfilePhoto
            });

            //排序
            var mappingDictionary = _propertyMappingService.GetPropertyMapping<ArticleListViewModel, Article>();
            list = list.ApplySort(parameters.Orderby, mappingDictionary);

            return await PageList<ArticleListViewModel>.CreatePageMsgAsync(list, parameters.PageNumber, parameters.PageSize);
        }
    }
}
