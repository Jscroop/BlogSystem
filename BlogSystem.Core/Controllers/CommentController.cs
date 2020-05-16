using BlogSystem.Core.Helpers;
using BlogSystem.IBLL;
using BlogSystem.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogSystem.Core.Controllers
{
    [ApiController]
    [Route("api/Article/{articleId}/Comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IArticleService _articleService;
        private readonly Guid _userId;

        public CommentController(ICommentService commentService, IArticleService articleService, IHttpContextAccessor httpContext)
        {
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            var accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(Guid articleId, CreateCommentViewModel model)
        {
            if (!await _articleService.ExistsAsync(articleId))
            {
                return NotFound();
            }

            await _commentService.CreateComment(model, articleId, _userId);
            return CreatedAtRoute(nameof(GetComments), new { articleId }, model);
        }

        /// <summary>
        /// 添加回复型评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="commentId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("reply")]
        public async Task<IActionResult> CreateReplyComment(Guid articleId, Guid commentId, CreateApplyCommentViewModel model)
        {
            if (!await _articleService.ExistsAsync(articleId))
            {
                return NotFound();
            }
            //回复的是正常评论
            if (await _commentService.ExistsAsync(commentId))
            {
                await _commentService.CreateReplyComment(model, articleId, commentId, _userId);
                return CreatedAtRoute(nameof(GetComments), new { articleId }, model);
            }
            //需要考虑回复的是正常评论还是回复型评论
            if (await _commentService.ReplyExistAsync(commentId))
            {
                await _commentService.CreateToReplyComment(model, articleId, commentId, _userId);
                return CreatedAtRoute(nameof(GetComments), new { articleId }, model);
            }
            return NotFound();
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetComments))]
        public async Task<IActionResult> GetComments(Guid articleId)
        {
            if (!await _articleService.ExistsAsync(articleId))
            {
                return NotFound();
            }

            var list = await _commentService.GetCommentsByArticleIdAsync(articleId);
            return Ok(list);
        }
    }
}
