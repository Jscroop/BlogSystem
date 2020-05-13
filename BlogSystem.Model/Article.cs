using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Model
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : BaseEntity
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        [Required, Column(TypeName = "text")]
        public string Content { get; set; }
        /// <summary>
        /// 发表人的Id，用户表的外键
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 看好人数
        /// </summary>
        public int GoodCount { get; set; }
        /// <summary>
        /// 不看好人数
        /// </summary>
        public int BadCount { get; set; }
        /// <summary>
        /// 文章查看所需等级
        /// </summary>
        public Level Level { get; set; } = Level.普通用户;
    }
}
