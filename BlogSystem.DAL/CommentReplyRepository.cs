using BlogSystem.IDAL;
using BlogSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class CommentReplyRepository : BaseRepository<CommentReply>, ICommentReplyRepository
    {
        public CommentReplyRepository() : base(new BlogSystemContext(new DbContextOptions<BlogSystemContext>()))
        {

        }
    }
}
