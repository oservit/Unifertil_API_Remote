using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public class GenericValidator
{
    public static List<string> Validate<T>(T model)
    {
        var errors = new List<string>();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null)
            {
                var value = property.GetValue(model);

                if (IsInvalidValue(value))
                {
                    errors.Add(requiredAttribute.ErrorMessage ?? $"O campo {property.Name} é obrigatório.");
                }
            }
        }

        return errors;
    }

    private static bool IsInvalidValue(object? value)
    {
        if (value == null)
            return true;

        if (value is string strValue)
            return string.IsNullOrWhiteSpace(strValue);

        if (value is int intValue)
            return intValue == 0;

        if (value is long longValue)
            return longValue == 0;

        if (value is IEnumerable<object> collection)
            return !collection.Any();

        return false;
    }
}