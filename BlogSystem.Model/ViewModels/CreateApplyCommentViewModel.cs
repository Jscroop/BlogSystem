using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 添加回复型评论
    /// </summary>
    public class CreateApplyCommentViewModel
    {
        /// <summary>
        /// 回复的内容
        /// </summary>
        [Required, StringLength(800)]
        public string Content { get; set; }
    }
}
