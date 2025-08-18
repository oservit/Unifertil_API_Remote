using System.Linq;
using System.Reflection;

namespace Libs
{
    public static class PropertyHelper
    {
        public static PropertyInfo GetPropertyIgnoreCase<T>(string propertyName) where T : class
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
