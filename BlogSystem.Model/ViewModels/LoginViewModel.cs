using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required, StringLength(40, MinimumLength = 4)]
        [RegularExpression(@"/^([\u4e00-\u9fa5]{2,4})|([A-Za-z0-9_]{4,16})|([a-zA-Z0-9_\u4e00-\u9fa5]{3,16})$/")]
        public string Account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
