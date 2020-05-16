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
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly Guid _userId;

        public CategoryController(ICategoryService categoryService, IHttpContextAccessor httpContext)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            var accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }

        /// <summary>
        /// 查询用户的文章分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}", Name = nameof(GetCategoryByUserId))]
        public async Task<IActionResult> GetCategoryByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return NotFound();
            }
            var list = await _categoryService.GetCategoryByUserIdAsync(userId);
            return Ok(list);
        }

        /// <summary>
        /// 新增文章分类
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]string categoryName)
        {
            var categoryId = await _categoryService.CreateCategory(categoryName, _userId);
            if (categoryId == Guid.Empty)
            {
                return BadRequest("重复分类！");
            }
            //创建成功返回查询页面链接
            var category = new CreateCategoryViewModel { CategoryId = categoryId, CategoryName = categoryName };
            return CreatedAtRoute(nameof(GetCategoryByUserId), new { userId = _userId }, category);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> RemoveCategory(Guid categoryId)
        {
            //确认是否存在，操作人与归属人是否一致
            var category = await _categoryService.GetOneByIdAsync(categoryId);
            if (category == null || category.UserId != _userId)
            {
                return NotFound();
            }

            await _categoryService.RemoveAsync(categoryId);
            return NoContent();
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel model)
        {
            if (!await _categoryService.EditCategory(model, _userId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
