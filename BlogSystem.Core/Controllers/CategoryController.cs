using BlogSystem.Core.Helpers;
using BlogSystem.IBLL;
using BlogSystem.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSystem.Core.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _accessor;
        private readonly Guid _userId;

        public CategoryController(ICategoryService categoryService, IHttpContextAccessor accessor)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));

            //获取用户Id
            var jwtStr = _accessor.HttpContext.Request.Headers["Authorization"].ToString();
            _userId = JwtHelper.JwtDecrypt(jwtStr).UserId;
        }

        /// <summary>
        /// 查询用户分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetCategoryByUserId))]
        public async Task<ActionResult<IEnumerable<CategoryListViewModel>>> GetCategoryByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return NotFound();
            }

            var list = await _categoryService.GetCategoryByUserIdAsync(userId);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpPost(nameof(CreateCategory))]
        public async Task<IActionResult> CreateCategory(string categoryName)
        {
            await _categoryService.CreateCategory(new CreateCategoryViewModel
            {
                UserId = _userId,
                CategoryName = categoryName
            });
            //创建成功返回分类查询页面
            return CreatedAtRoute(nameof(GetCategoryByUserId), _userId);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete(nameof(RemoveCategory))]
        public async Task<IActionResult> RemoveCategory(Guid categoryId)
        {
            if (!await _categoryService.ExistsAsync(categoryId))
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
        [HttpPatch(nameof(EditCategory))]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel model)
        {
            if (!await _categoryService.ExistsAsync(model.CategoryId))
            {
                return NotFound();
            }
            await _categoryService.EditCategory(model);
            return NoContent();
        }
    }
}
