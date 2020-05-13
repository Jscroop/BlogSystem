using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Model
{
    /// <summary>
    /// 用户关注表
    /// </summary>
    public class UserFocus : BaseEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        /// <summary>
        /// 关注用户编号
        /// </summary>
        [ForeignKey(nameof(Focus))]
        public Guid FocusId { get; set; }
        public User Focus { get; set; }
    }
}
