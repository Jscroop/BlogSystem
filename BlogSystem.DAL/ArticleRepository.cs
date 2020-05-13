using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {
        }
    }
}
