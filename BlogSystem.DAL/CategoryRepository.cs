using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {

        }
    }
}
