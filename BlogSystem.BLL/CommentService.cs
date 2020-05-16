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
    public class CommentService : BaseService<ArticleComment>, ICommentService
    {
        private readonly IArticleCommentRepository _commentRepository;
        private readonly ICommentReplyRepository _commentReplyRepository;

        public CommentService(IArticleCommentRepository commentRepository, ICommentReplyRepository commentReplyRepository)
        {
            _commentRepository = commentRepository;
            BaseRepository = commentRepository;
            _commentReplyRepository = commentReplyRepository;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateComment(CreateCommentViewModel model, Guid articleId, Guid userId)
        {
            await _commentRepository.CreateAsync(new ArticleComment()
            {
                ArticleId = articleId,
                Content = model.Content,
                UserId = userId
            });
        }

        /// <summary>
        ///  添加普通评论的回复
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleId"></param>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateReplyComment(CreateApplyCommentViewModel model, Guid articleId, Guid commentId, Guid userId)
        {
            var comment = await _commentRepository.GetOneByIdAsync(commentId);
            var toUserId = comment.UserId;

            await _commentReplyRepository.CreateAsync(new CommentReply()
            {
                CommentId = commentId,
                ToUserId = toUserId,
                ArticleId = articleId,
                UserId = userId,
                Content = model.Content
            });
        }

        /// <summary>
        /// 添加回复型评论的回复
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleId"></param>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateToReplyComment(CreateApplyCommentViewModel model, Guid articleId, Guid commentId, Guid userId)
        {
            var comment = await _commentReplyRepository.GetOneByIdAsync(commentId);
            var toUserId = comment.UserId;

            await _commentReplyRepository.CreateAsync(new CommentReply()
            {
                CommentId = commentId,
                ToUserId = toUserId,
                ArticleId = articleId,
                UserId = userId,
                Content = model.Content
            });
        }

        /// <summary>
        /// 根据文章Id获取评论信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<CommentListViewModel>> GetCommentsByArticleIdAsync(Guid articleId)
        {
            //正常评论
            var comment = await _commentRepository.GetAll().Where(m => m.ArticleId == articleId)
                .Include(m => m.User).Select(m => new CommentListViewModel
                {
                    ArticleId = m.ArticleId,
                    UserId = m.UserId,
                    Account = m.User.Account,
                    ProfilePhoto = m.User.ProfilePhoto,
                    CommentId = m.Id,
                    CommentContent = m.Content,
                    CreateTime = m.CreateTime
                }).ToListAsync();

            //回复型的评论
            var replyComment = await _commentReplyRepository.GetAll().Where(m => m.ArticleId == articleId)
                .Include(m => m.User).Select(m => new CommentListViewModel
                {
                    ArticleId = m.ArticleId,
                    UserId = m.UserId,
                    Account = m.User.Account,
                    ProfilePhoto = m.User.ProfilePhoto,
                    CommentId = m.Id,
                    CommentContent = $"@{m.ToUser.Account}" + Environment.NewLine + m.Content,
                    CreateTime = m.CreateTime
                }).ToListAsync();

            var list = comment.Union(replyComment).OrderByDescending(m => m.CreateTime).ToList();
            return list;
        }

        /// <summary>
        /// 确认回复型评论是否存在
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<bool> ReplyExistAsync(Guid commentId)
        {
            return await _commentReplyRepository.GetAll().AnyAsync(m => m.Id == commentId);
        }
    }
}
