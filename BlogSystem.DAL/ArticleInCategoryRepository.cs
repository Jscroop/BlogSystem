using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class ArticleInCategoryRepository : BaseRepository<ArticleInCategory>, IArticleInCategoryRepository
    {
        public ArticleInCategoryRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {
        }
    }
}
