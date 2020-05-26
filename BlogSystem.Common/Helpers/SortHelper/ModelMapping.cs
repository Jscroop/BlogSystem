using System;
using System.Collections.Generic;

namespace BlogSystem.Common.Helpers.SortHelper
{
    //定义模型对象之间的映射关系，如xxx对应xxxDto
    public class ModelMapping<TSource, TDestination> : IModelMapping
    {
        public Dictionary<string, PropertyMapping> MappingDictionary { get; private set; }

        public ModelMapping(Dictionary<string, PropertyMapping> mappingDictionary)
        {
            MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
        }
    }
}
