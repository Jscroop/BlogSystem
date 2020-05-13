using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 创建文章
    /// </summary>
    public class CreateArticleViewModel
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 文章分类
        /// </summary>
        public List<Guid> CategoryIds { get; set; }
    }
}
