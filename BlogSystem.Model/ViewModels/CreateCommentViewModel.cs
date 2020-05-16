using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 文章评论
    /// </summary>
    public class CreateCommentViewModel
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        [Required, StringLength(800)]
        public string Content { get; set; }
    }
}
