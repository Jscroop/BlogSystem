using System;

namespace BlogSystem.Model.Parameters
{
    public class ArticleParameters
    {
        public string Orderby { get; set; } = "CreateTime";

        //每页的最大数量
        private const int MaxPageSize = 20;

        //过滤条件——距离时间
        public DistanceTime DistanceTime { get; set; }

        //搜索条件
        public string SearchStr { get; set; }

        //数据塑形字段
        public string Fields { get; set; }

        //页码数
        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value <= 0) ? 1 : value;
        }

        //每页数量
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > MaxPageSize) { _pageSize = MaxPageSize; }
                else if (value <= 0) { _pageSize = 10; }
                else { _pageSize = value; }
            }
        }
    }

    public enum DistanceTime
    {
        Year = 1,
        Month = 2,
        Week = 3,
    }
}
