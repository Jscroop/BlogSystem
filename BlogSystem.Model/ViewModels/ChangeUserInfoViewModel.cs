using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 修改用户资料
    /// </summary>
    public class ChangeUserInfoViewModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime BirthOfDate { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
    }
}
