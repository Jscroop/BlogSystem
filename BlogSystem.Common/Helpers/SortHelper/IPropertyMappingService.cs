using System.Collections.Generic;

namespace BlogSystem.Common.Helpers.SortHelper
{
    //实现依赖注入新建的接口——对应的是属性映射服务
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMapping> GetPropertyMapping<TSource, TDestination>();
        bool PropertyMappingExist<TSource, TDestination>(string fields);
    }
}
