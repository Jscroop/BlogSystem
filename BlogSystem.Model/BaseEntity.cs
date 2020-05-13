using System;

namespace BlogSystem.Model
{
    /// <summary>
    /// model的基类
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 唯一标识Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除(伪删除)
        /// </summary>
        public bool IsRemoved { get; set; }
    }
}
