using BlogSystem.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogSystem.Model.ViewModels;

namespace BlogSystem.IBLL
{
    /// <summary>
    /// 评论服务接口
    /// </summary>
    public interface ICommentService : IBaseService<ArticleComment>
    {
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateComment(CreateCommentViewModel model, Guid articleId, Guid userId);

        /// <summary>
        /// 添加普通评论的回复
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleId"></param>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateReplyComment(CreateApplyCommentViewModel model, Guid articleId, Guid commentId, Guid userId);

        /// <summary>
        /// 添加回复评论的回复
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleId"></param>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateToReplyComment(CreateApplyCommentViewModel model, Guid articleId, Guid commentId, Guid userId);

        /// <summary>
        /// 通过文章Id获取所有评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<CommentListViewModel>> GetCommentsByArticleIdAsync(Guid articleId);

        /// <summary>
        /// 确认回复型评论是否存在
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<bool> ReplyExistAsync(Guid commentId);
    }
}
