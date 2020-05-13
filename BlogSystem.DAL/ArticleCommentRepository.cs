using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class ArticleCommentRepository : BaseRepository<ArticleComment>, IArticleCommentRepository
    {
        public ArticleCommentRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {
        }
    }
}
