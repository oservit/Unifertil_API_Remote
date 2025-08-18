using System.Collections;
using System.Dynamic;
using System.Reflection;

namespace Libs
{
    public static class DynamicHelper
    {

        public static ExpandoObject ConvertToExpandoObject<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), "O objeto não pode ser nulo.");

            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)expando;

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead) continue;

                var value = property.GetValue(obj);
                expandoDict.Add(property.Name, value);
            }

            return expando;
        }


        public static List<T> ConvertToList<T>(List<object> sourceList) where T : class
        {
            var resultList = new List<T>();

            foreach (var item in sourceList)
            {
                if (item is T typedItem)
                {
                    resultList.Add(typedItem);
                }
                else
                {
                    try
                    {
                        var convertedItem = (T)Convert.ChangeType(item, typeof(T));
                        resultList.Add(convertedItem);
                    }
                    catch (InvalidCastException)
                    {
                        throw new InvalidCastException($"Cannot convert item of type {item.GetType()} to {typeof(T)}.");
                    }
                }
            }

            return resultList;
        }

        public static T Clone<T>(T original) where T : class
        {
            if (original == null)
                throw new ArgumentNullException(nameof(original));

            var newObject = (T)Activator.CreateInstance(original.GetType());

            CopyProperties(original, newObject);

            return newObject;
        }

        private static void CopyProperties<T>(T source, T destination) where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var property in properties)
            {
                var value = property.GetValue(source);

                if (value != null && property.PropertyType.IsGenericType &&
                    typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var newCollection = CloneCollection(value);
                    property.SetValue(destination, newCollection);
                }
                else
                    property.SetValue(destination, value);
            }
        }

        private static object CloneCollection(object originalCollection)
        {
            var elementType = originalCollection.GetType().GetGenericArguments()[0];

            var newCollection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var item in (IEnumerable)originalCollection)
            {
                var clonedItem = Clone(item);
                newCollection.Add(clonedItem);
            }

            return newCollection;
        }
        public static object ConvertToCompatibleType(object value, Type targetType)
        {
            if (value == null)
                return null;

            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                if (value == DBNull.Value)
                    return null;

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType));
            }

            return Convert.ChangeType(value, targetType);
        }
    }
}
