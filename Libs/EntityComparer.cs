using System.Reflection;

namespace Libs
{
    public class EntityComparer
    {
        public static EntityComparison CompareEntities(object originalEntity, object updatedEntity)
        {
            if (originalEntity == null || updatedEntity == null)
                throw new ArgumentNullException("As entidades não podem ser nulas.");

            var properties = originalEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(p => p.CanRead && p.CanWrite)
                                            .ToList();

            var originalValuesDict = new Dictionary<string, object>();
            var updatedValuesDict = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var originalValue = property.GetValue(originalEntity);
                var updatedValue = property.GetValue(updatedEntity);

                if (IsPrimitiveOrString(property.PropertyType))
                {
                    if (!Equals(originalValue, updatedValue))
                    {
                        originalValuesDict[property.Name] = originalValue;
                        updatedValuesDict[property.Name] = updatedValue;
                    }
                }
            }

            return new EntityComparison
            {
                OriginalEntity = originalValuesDict,
                UpdatedEntity = updatedValuesDict
            };
        }

        private static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) || Nullable.GetUnderlyingType(type) != null;
        }
    }

    public class EntityComparison
    {
        public object OriginalEntity { get; set; }
        public object UpdatedEntity { get; set; }
    }
}
