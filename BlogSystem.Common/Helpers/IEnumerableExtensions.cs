using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace BlogSystem.Common.Helpers
{
    //数据塑形——针对集合的扩展方法
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeDataList<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>(source.Count());

            var propertyInfoList = new List<PropertyInfo>();

            //field无字段则反射全部
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else //field有字段则去除空格并判断后添加至list
            {
                var fieldAfterSplit = fields.Split(",");
                foreach (var field in fieldAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo =
                        typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase
                                                                  | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property:{propertyName}没有找到：{typeof(TSource)}");
                    }
                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (TSource obj in source)
            {
                var shapedObj = new ExpandoObject();
                //根据获取的属性额值添加到shapedObj中
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(obj);
                    ((IDictionary<string, object>)shapedObj).Add(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(shapedObj);
            }
            return expandoObjectList;

        }
    }
}
