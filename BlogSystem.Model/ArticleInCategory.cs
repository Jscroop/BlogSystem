using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Model
{
    /// <summary>
    /// 文章所属分类
    /// </summary>
    public class ArticleInCategory : BaseEntity
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        /// <summary>
        /// 文章Id
        /// </summary>
        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}