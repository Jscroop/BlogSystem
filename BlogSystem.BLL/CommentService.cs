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
        /// <returns></returns>
        public async Task CreateComment(ArticleComment model)
        {
            await _commentRepository.CreateAsync(model);
        }

        /// <summary>
        /// 添加回复型评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateReplyComment(CommentReply model)
        {
            await _commentReplyRepository.CreateAsync(model);
        }

        /// <summary>
        /// 根据文章Id获取评论信息，需要考虑回复型评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<CommentListViewModel>> GetCommentsByArticleIdAsync(Guid articleId)
        {
            return await _commentRepository.GetAllByOrder(false).Where(m => m.ArticleId == articleId)
                    .Include(m => m.User).Select(m => new CommentListViewModel
                    {
                        ArticleId = m.ArticleId,
                        UserId = m.UserId,
                        Account = m.User.Account,
                        CommentId = m.Id,
                        CommentContent = m.Content,
                        CreateTime = m.CreateTime
                    }).ToListAsync();
        }
    }
}
