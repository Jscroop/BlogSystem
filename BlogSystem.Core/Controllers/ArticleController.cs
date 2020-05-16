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
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly Guid _userId;

        public ArticleController(IArticleService articleService, IHttpContextAccessor httpContext)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            var accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleViewModel model)
        {

            var articleId = await _articleService.CreateArticleAsync(model, _userId);
            return CreatedAtRoute(nameof(GetArticleByArticleId), new { articleId }, model);
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> EditArticle(EditArticleViewModel model)
        {
            if (!await _articleService.EditArticleAsync(model, _userId))
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{articleId}")]
        public async Task<IActionResult> RemoveArticle(Guid articleId)
        {
            //删除文章所属分类
            if (!await _articleService.RemoveArticleInCategory(articleId, _userId))
            {
                return NotFound();
            }
            //删除文章
            await _articleService.RemoveAsync(articleId);
            return NoContent();
        }

        /// <summary>
        /// 通过文章Id获取文章详情
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet("{articleId}", Name = nameof(GetArticleByArticleId))]
        public async Task<IActionResult> GetArticleByArticleId(Guid articleId)
        {
            var article = await _articleService.GetArticleDetailsByArticleIdAsync(articleId);
            if (article.Id == Guid.Empty)
            {
                return NotFound();
            }
            return Ok(article);
        }

        /// <summary>
        /// 通过用户Id获取文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetArticlesByUserId(Guid userId)
        {
            var list = await _articleService.GetArticlesByUserIdAsync(userId);
            return Ok(list);
        }

        /// <summary>
        /// 通过用户分类Id获取文章列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("list/{userId}/{categoryId}")]
        public async Task<IActionResult> GetArticlesByCategoryId(Guid userId, Guid categoryId)
        {
            var list = await _articleService.GetArticlesByCategoryIdAsync(userId, categoryId);
            return Ok(list);
        }
    }
}
