using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// 账户
        /// </summary>
        [Required, StringLength(40)]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required, StringLength(200)]
        public string Password { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string ProfilePhoto { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthOfDate { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public Level Level { get; set; } = Level.普通用户;
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FansNum { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public int FocusNum { get; set; }
    }
}
