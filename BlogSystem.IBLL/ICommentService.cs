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
        /// <returns></returns>
        Task CreateComment(ArticleComment model);

        /// <summary>
        /// 添加回复型评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateReplyComment(CommentReply model);

        /// <summary>
        /// 通过文章Id获取所有评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<CommentListViewModel>> GetCommentsByArticleIdAsync(Guid articleId);
    }
}
