using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 创建文章分类
    /// </summary>
    public class CreateCategoryViewModel
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required, StringLength(30, MinimumLength = 2)]
        public string CategoryName { get; set; }
    }
}
