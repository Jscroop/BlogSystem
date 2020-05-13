using System;

namespace BlogSystem.Model.ViewModels
{
    public class CategoryListViewModel
    {
        
        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
    }
}
