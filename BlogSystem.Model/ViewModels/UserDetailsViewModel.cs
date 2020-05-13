using System;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 用户详细信息-点击查看主页
    /// </summary>
    public class UserDetailsViewModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string ProfilePhoto { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public Level Level { get; set; }
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
