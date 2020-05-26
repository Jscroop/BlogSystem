using System;
using System.Collections.Generic;

namespace BlogSystem.Common.Helpers.SortHelper
{
    //定义属性之间的映射关系
    public class PropertyMapping
    {
        //针对可能出现的一对多的情况——如name对应的是firstName+lastName
        public IEnumerable<string> DestinationProperties { get; set; }

        //针对出生日期靠前但是对应年龄大的情况
        public bool Revert { get; set; }

        public PropertyMapping(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}
