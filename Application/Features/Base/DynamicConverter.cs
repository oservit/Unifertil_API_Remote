using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Application.Features.Base
{
    public enum PropertyNameFormat
    {
        CamelCase,
        DefaultFormat
    }

    public static class DynamicConverter
    {
        public static dynamic ConvertToExpandoObject(object obj, PropertyNameFormat format = PropertyNameFormat.CamelCase)
        {
            var expando = new ExpandoObject();
            var dict = expando as IDictionary<string, object>;

            if (dict == null)
                throw new InvalidOperationException("Failed to cast ExpandoObject to IDictionary<string, object>.");

            foreach (var property in obj.GetType().GetProperties())
            {
                if (!property.CanRead) continue;

                object value;
                try
                {
                    value = property.GetValue(obj);
                }
                catch
                {
                    continue;
                }

                var propertyName = GetPropertyName(property.Name, format);

                bool isNullable = property.PropertyType.IsGenericType &&
                                  property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                if (value == null)
                {
                    if (isNullable || !property.PropertyType.IsValueType)
                        dict[propertyName] = null;
                    continue;
                }

                if (isNullable)
                {
                    var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (underlyingType != null)
                        dict[propertyName] = Convert.ChangeType(value, underlyingType);
                }
                else if (property.PropertyType.IsEnum)
                    dict[propertyName] = Convert.ToInt32(value);
                else if (value is IEnumerable enumerable && !(value is string))
                {
                    var list = new List<object>();
                    foreach (var item in enumerable)
                        list.Add(ConvertToExpandoObject(item, format));
                    dict[propertyName] = list;
                }
                else if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                    dict[propertyName] = ConvertToExpandoObject(value, format);
                else
                    dict[propertyName] = value;
            }

            return expando;
        }

        private static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }

        private static string ConvertToDefaultFormat(string propertyName) => propertyName;

        private static string GetPropertyName(string propertyName, PropertyNameFormat format) =>
            format switch
            {
                PropertyNameFormat.CamelCase => ToCamelCase(propertyName),
                PropertyNameFormat.DefaultFormat => ConvertToDefaultFormat(propertyName),
                _ => propertyName
            };
    }
}
