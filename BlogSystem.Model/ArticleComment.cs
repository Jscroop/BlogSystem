using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Model
{
    /// <summary>
    /// 文章评论表
    /// </summary>
    public class ArticleComment : BaseEntity
    {
        /// <summary>
        /// 评论的文章ID
        /// </summary>
        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        /// <summary>
        /// 评论用户ID
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Required, StringLength(800)]
        public string Content { get; set; }
    }
}