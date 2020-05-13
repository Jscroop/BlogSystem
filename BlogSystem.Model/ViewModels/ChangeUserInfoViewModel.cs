using System;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 修改用户资料
    /// </summary>
    public class ChangeUserInfoViewModel
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
        /// 出生日期
        /// </summary>
        public DateTime BirthOfDate { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
    }
}
