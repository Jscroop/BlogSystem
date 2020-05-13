using System;

namespace BlogSystem.Model.ViewModels
{
    public class CommentListViewModel
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 评论Id
        /// </summary>
        public Guid CommentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
