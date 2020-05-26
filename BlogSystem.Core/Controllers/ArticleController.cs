using BlogSystem.Common.Helpers;
using BlogSystem.Common.Helpers.SortHelper;
using BlogSystem.Core.Helpers;
using BlogSystem.IBLL;
using BlogSystem.Model;
using BlogSystem.Model.HATEOAS;
using BlogSystem.Model.Helpers;
using BlogSystem.Model.Parameters;
using BlogSystem.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogSystem.Core.Controllers
{
    [ApiController]
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckService _propertyCheckService;
        private readonly Guid _userId;

        public ArticleController(IArticleService articleService, IHttpContextAccessor httpContext, IPropertyMappingService propertyMappingService,
            IPropertyCheckService propertyCheckService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckService = propertyCheckService ?? throw new ArgumentNullException(nameof(propertyCheckService));
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
        [HttpPatch(Name = nameof(EditArticle))]
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
        [HttpDelete("{articleId}", Name = nameof(RemoveArticle))]
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
        /// <param name="fields"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        [HttpGet("{articleId}", Name = nameof(GetArticleByArticleId))]
        public async Task<IActionResult> GetArticleByArticleId(Guid articleId, string fields,
        [FromHeader(Name = "Accept")] string mediaType)//获取媒体类型的数值
        {
            //解析媒体资源类型
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parseMediaType))
            {
                return BadRequest();
            }

            //确认是否存在该字段
            if (!_propertyCheckService.TypeHasProperties<ArticleDetailsViewModel>(fields))
            {
                return BadRequest();
            }

            var article = await _articleService.GetArticleDetailsByArticleIdAsync(articleId);
            if (article.Id == Guid.Empty)
            {
                return NotFound();
            }

            //expandoObject实质上是一个字典
            var result = article.ShapeData(fields) as IDictionary<string, object>;

            //判断媒体类型并返回不同的结果
            if (parseMediaType.MediaType == "application/vnd.company.hateoas+json")
            {
                var link = CreateLinksForArticle(articleId, fields);
                result.Add("links", link);
                return Ok(result);
            }

            return Ok(result);
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

        /// <summary>
        /// 过滤/搜索文章信息并返回list和分页信息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("search", Name = nameof(GetArticles))]
        public async Task<IActionResult> GetArticles([FromQuery]ArticleParameters parameters)
        {
            //不存在排序字段则返回400错误
            if (!_propertyMappingService.PropertyMappingExist<ArticleListViewModel, Article>(parameters.Orderby))
            {
                return BadRequest();
            }

            //确认是否存在该字段
            if (!_propertyCheckService.TypeHasProperties<ArticleListViewModel>(parameters.Fields))
            {
                return BadRequest();
            }

            var list = await _articleService.GetArticles(parameters);

            //前后页信息
            //var previousPageLink = list.HasPrevious ? CreateArticleUrl(parameters, UrlType.PreviousPage) : null;

            //var nextPageLink = list.HasNext ? CreateArticleUrl(parameters, UrlType.NextPage) : null;

            var paginationX = new
            {
                totalCount = list.TotalCount,
                pageSize = list.PageSize,
                currentPage = list.CurrentPage,
                totalPages = list.TotalPages,
                //previousPageLink,
                //nextPageLink
            };
            //添加前后页面信息
            Response.Headers.Add("Pagination-X", JsonSerializer.Serialize(paginationX, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            //防止添加hateoas异常
            if (!string.IsNullOrWhiteSpace(parameters.Fields))
            {
                if (!parameters.Fields.ToLowerInvariant().Contains("articleid"))
                {
                    parameters.Fields += ",articleid";
                }
            }

            //结果集数据塑形
            var shapedList = list.ShapeDataList(parameters.Fields);

            //集合内部元素依次添加links信息
            var resultList = shapedList.Select(x =>
            {
                var articleDict = x as IDictionary<string, object>;
                var articleLinks = CreateLinksForArticle((Guid)articleDict["ArticleId"], null);
                articleDict.Add("links", articleLinks);
                return articleDict;
            });

            //为集合资源获取HEATOAS的Links信息
            var links = CreateLinksForArticles(parameters, list.HasPrevious, list.HasNext);

            //组合信息并返回
            var resultListWithLinks = new
            {
                value = resultList,
                links
            };

            return Ok(resultListWithLinks);
        }

        //返回前一页面，后一页，以及当前页的url信息
        private string CreateArticleUrl(ArticleParameters parameters, UrlType type)
        {
            var isDefined = Enum.IsDefined(typeof(DistanceTime), parameters.DistanceTime);

            switch (type)
            {
                case UrlType.PreviousPage:
                    return Url.Link(nameof(GetArticles), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.Orderby,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        distanceTime = isDefined ? parameters.DistanceTime.ToString() : null,
                        searchStr = parameters.SearchStr
                    });
                case UrlType.NextPage:
                    return Url.Link(nameof(GetArticles), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.Orderby,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        distanceTime = isDefined ? parameters.DistanceTime.ToString() : null,
                        searchStr = parameters.SearchStr
                    });
                default:
                    return Url.Link(nameof(GetArticles), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.Orderby,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        distanceTime = isDefined ? parameters.DistanceTime.ToString() : null,
                        searchStr = parameters.SearchStr
                    });
            }
        }

        //实现HATEOAS单个资源的简单方法
        private IEnumerable<LinkDto> CreateLinksForArticle(Guid articleId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link(nameof(GetArticleByArticleId), new { articleId }), "self", "Get"));
            }
            else
            {
                links.Add(new LinkDto(Url.Link(nameof(GetArticleByArticleId), new { articleId, fields }), "self", "Get"));
            }

            //删除文章的 link
            links.Add(new LinkDto(Url.Link(nameof(RemoveArticle), new { articleId, fields }), "delete_article need_auth", "DELETE"));

            //编辑文章的 link
            links.Add(new LinkDto(Url.Link(nameof(EditArticle), new { articleId }), "edit_article need_auth", "PATCH"));

            return links;
        }

        //实现HATEOAS集合资源的简单方法，将自身的前一页信息和后一页信息也放到headoas中
        private IEnumerable<LinkDto> CreateLinksForArticles(ArticleParameters parameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateArticleUrl(parameters, UrlType.CurrentPage), "self", "GET")
            };

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateArticleUrl(parameters, UrlType.PreviousPage), "Previous", "GET"));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateArticleUrl(parameters, UrlType.NextPage), "Next", "GET"));
            }

            return links;
        }


    }
}
