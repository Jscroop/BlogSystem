using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class UserFocusRepository : BaseRepository<UserFocus>, IUserFocusRepository
    {
        public UserFocusRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {

        }
    }
}
