using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Model;
using BlogSystem.Model.Helpers;
using BlogSystem.Model.Parameters;
using BlogSystem.Model.ViewModels;

namespace BlogSystem.IBLL
{
    /// <summary>
    /// 文章接口
    /// </summary>
    public interface IArticleService : IBaseService<Article>
    {
        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Guid> CreateArticleAsync(CreateArticleViewModel model, Guid userId);

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> EditArticleAsync(EditArticleViewModel model, Guid userId);

        /// <summary>
        /// 通过Id获取文章详情
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<ArticleDetailsViewModel> GetArticleDetailsByArticleIdAsync(Guid articleId);

        /// <summary>
        /// 通过用户Id获取文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ArticleListViewModel>> GetArticlesByUserIdAsync(Guid userId);

        /// <summary>
        /// 通过用户分类Id获取所有文章
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<ArticleListViewModel>> GetArticlesByCategoryIdAsync(Guid userId, Guid categoryId);

        /// <summary>
        /// 通过用户Id获取文章数量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<int> GetArticleCountByUserIdAsync(Guid userid);

        /// <summary>
        /// 点赞文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task AddGoodCount(Guid articleId, Guid userId);

        /// <summary>
        /// 点灭文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task AddBadCount(Guid articleId, Guid userId);

        /// <summary>
        /// 删除文章所属分类信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> RemoveArticleInCategory(Guid articleId, Guid userId);

        /// <summary>
        /// 新增文章所属分类信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        Task CreateArticleInCategory(Guid articleId, List<Guid> categoryIds);

        /// <summary>
        /// 文章过滤及搜索
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PageList<ArticleListViewModel>> GetArticles(ArticleParameters parameters);
    }
}
