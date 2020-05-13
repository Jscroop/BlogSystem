using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Model;
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
        /// <returns></returns>
        Task CreateArticleAsync(CreateArticleViewModel model);

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditArticleAsync(EditArticleViewModel model);

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
        /// 通过分类Id获取所有文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<ArticleListViewModel>> GetArticlesByCategoryIdAsync(Guid categoryId);

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
        /// <returns></returns>
        Task AddGoodCount(Guid articleId);

        /// <summary>
        /// 点灭文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task AddBadCount(Guid articleId);
    }
}
