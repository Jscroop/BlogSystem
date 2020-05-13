using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 编辑文章
    /// </summary>
    public class EditArticleViewModel
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid Id { get; set; }

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
