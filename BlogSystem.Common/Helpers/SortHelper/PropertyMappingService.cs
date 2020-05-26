using BlogSystem.Model;
using BlogSystem.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogSystem.Common.Helpers.SortHelper
{
    //属性映射处理
    public class PropertyMappingService : IPropertyMappingService
    {
        //一个只读属性的字典，里面是Dto和数据库表字段的映射关系
        private readonly Dictionary<string, PropertyMapping> _articlePropertyMapping
            = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase) //忽略大小写
            {
                {"Id",new PropertyMapping(new List<string>{"Id"}) },
                {"Title",new PropertyMapping(new List<string>{"Title"}) },
                {"Content",new PropertyMapping(new List<string>{"Content"}) },
                {"CreateTime",new PropertyMapping(new List<string>{"CreateTime"}) }
            };

        //需要解决ModelMapping泛型关系无法建立问题，可新增的一个空的标志接口
        private readonly IList<IModelMapping> _propertyMappings = new List<IModelMapping>();

        //构造函数——内部添加的是类和类的映射关系以及属性和属性的映射关系
        public PropertyMappingService()
        {
            _propertyMappings.Add(new ModelMapping<ArticleListViewModel, Article>(_articlePropertyMapping));
        }

        //通过两个类的类型获取映射关系
        public Dictionary<string, PropertyMapping> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<ModelMapping<TSource, TDestination>>();
            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count == 1)
            {
                return propertyMappings.First().MappingDictionary;
            }
            throw new Exception($"无法找到唯一的映射关系：{typeof(TSource)},{typeof(TDestination)}");
        }

        //判断字符串是否存在对应的字段
        public bool PropertyMappingExist<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            //查询字符串逗号分隔
            var fieldAfterSplit = fields.Split(",");
            foreach (var field in fieldAfterSplit)
            {
                var trimmedFields = field.Trim();//字段去空
                var indexOfFirstSpace = trimmedFields.IndexOf(" ", StringComparison.Ordinal);//获取字段中第一个空格的索引
                //空格不存在，则属性名为其本身，否则移除空格
                var propertyName = indexOfFirstSpace == -1 ? trimmedFields : trimmedFields.Remove(indexOfFirstSpace);
                //只要有一个字段对应不上就返回fasle
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
