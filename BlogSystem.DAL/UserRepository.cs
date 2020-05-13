using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {

        }
    }
}
