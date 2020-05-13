using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogSystem.Model.ViewModels
{
    public class EditCategoryViewModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        public string CategoryName { get; set; }
    }
}
