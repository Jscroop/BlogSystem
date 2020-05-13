using System;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 文章列表
    /// </summary>
    public class ArticleListViewModel
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string ProfilePhoto { get; set; }

    }
}
