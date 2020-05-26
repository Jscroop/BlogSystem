using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Model.Helpers
{
    public class PageList<T> : List<T>
    {
        //当前页码
        public int CurrentPage { get; }
        //总页码数
        public int TotalPages { get; }
        //每页数量
        public int PageSize { get; }
        //结果数量
        public int TotalCount { get; }
        //是否有前一页
        public bool HasPrevious => CurrentPage > 1;
        //是否有后一页
        public bool HasNext => CurrentPage < TotalPages;

        //初始化翻页信息
        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        //创建分页信息
        public static async Task<PageList<T>> CreatePageMsgAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}
