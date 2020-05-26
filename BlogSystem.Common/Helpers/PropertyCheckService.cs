using System.Reflection;

namespace BlogSystem.Common.Helpers
{
    //判断属性是否存在的服务
    public class PropertyCheckService : IPropertyCheckService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldAfterSplit = fields.Split(",");
            foreach (var field in fieldAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo =
                    typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase
                                                        | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
