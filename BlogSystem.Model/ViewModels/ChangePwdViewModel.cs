using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 修改用户密码
    /// </summary>
    public class ChangePwdViewModel
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required, Compare(nameof(NewPassword))]
        public string RequirePassword { get; set; }
    }
}
