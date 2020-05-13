using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Model
{
    /// <summary>
    /// 分类
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        public string CategoryName { get; set; }
        /// <summary>
        /// 分类对应的用户
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}