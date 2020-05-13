using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 修改用户头像
    /// </summary>
    public class ChangeUserPhotoViewModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Required]
        public string ProfilePhoto { get; set; }
    }
}
