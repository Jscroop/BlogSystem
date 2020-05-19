using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required, StringLength(40, MinimumLength = 4)]
        [RegularExpression(@"/^([\u4e00-\u9fa5]{2,4})|([A-Za-z0-9_]{4,16})|([a-zA-Z0-9_\u4e00-\u9fa5]{3,16})$/")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required, Compare(nameof(Password))]
        public string RequirePassword { get; set; }
    }
}
