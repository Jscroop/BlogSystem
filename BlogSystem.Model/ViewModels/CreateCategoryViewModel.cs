using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    public class CreateCategoryViewModel
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        public string CategoryName { get; set; }
    }
}
