using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlogSystem.Model
{
    public class BlogSystemContext : DbContext
    {
        public BlogSystemContext()
        {
        }

        public BlogSystemContext(DbContextOptions<BlogSystemContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //关闭级联删除
            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()).Where(x => x.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var foreign in foreignKeys)
            {
                foreign.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //配置数据库连接
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BlogSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleComment> ArticleComments { get; set; }

        public DbSet<ArticleInCategory> ArticleInCategories { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CommentReply> CommentReplies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserFocus> UserFocuses { get; set; }
    }
}
