using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 编辑分类
    /// </summary>
    public class EditCategoryViewModel
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
