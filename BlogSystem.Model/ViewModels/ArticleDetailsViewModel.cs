using System;
using System.Collections.Generic;

namespace BlogSystem.Model.ViewModels
{
    /// <summary>
    /// 文章详情
    /// </summary>
    public class ArticleDetailsViewModel
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid Id { get; set; }

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
        /// 作者
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string ProfilePhoto { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public List<Guid> CategoryIds { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public List<string> CategoryNames { get; set; }

        /// <summary>
        /// 看好人数
        /// </summary>
        public int GoodCount { get; set; }
        /// <summary>
        /// 不看好人数
        /// </summary>
        public int BadCount { get; set; }

    }
}
