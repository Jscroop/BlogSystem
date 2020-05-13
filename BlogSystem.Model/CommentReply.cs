using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Model
{
    /// <summary>
    /// 评论回复表
    /// </summary>
    public class CommentReply : BaseEntity
    {
        /// <summary>
        /// 回复指向的评论Id
        /// </summary>
        [ForeignKey(nameof(ArticleComment))]
        public Guid CommentId { get; set; }
        public ArticleComment ArticleComment { get; set; }
        /// <summary>
        /// 回复指向的用户Id
        /// </summary>
        [ForeignKey(nameof(ToUser))]
        public Guid ToUserId { get; set; }
        public User ToUser { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 回复的内容
        /// </summary>
        [Required, StringLength(800)]
        public string Content { get; set; }
    }
}