using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Libs
{
    public static class FilterOperations
    {
        public static Dictionary<OperationType, Func<Expression, Expression, Expression>> GetOperations()
        {
            return new Dictionary<OperationType, Func<Expression, Expression, Expression>>
        {
            { OperationType.Contains, (property, value) =>
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })
                    ?? throw new InvalidOperationException("The 'Contains' method was not found on the string type.");

                var lowerProperty = ApplyStringMethodIfString(property, "ToLower");
                var lowerValue = ApplyStringMethodIfString(value, "ToLower");

                return Expression.Call(lowerProperty, containsMethod, lowerValue);
            }},
            { OperationType.Equals, (property, value) =>
            {
                var lowerProperty = ApplyStringMethodIfString(property, "ToLower");
                var lowerValue = ApplyStringMethodIfString(value, "ToLower");

                return Expression.Equal(lowerProperty, lowerValue);
            }},
            { OperationType.NotEquals, (property, value) =>
            {
                var lowerProperty = ApplyStringMethodIfString(property, "ToLower");
                var lowerValue = ApplyStringMethodIfString(value, "ToLower");

                return Expression.NotEqual(lowerProperty, lowerValue);
            }},
            { OperationType.GreaterThan, (property, value) => Expression.GreaterThan(property, value) },
            { OperationType.GreaterThanOrEquals, (property, value) => Expression.GreaterThanOrEqual(property, value) },
            { OperationType.LessThan, (property, value) => Expression.LessThan(property, value) },
            { OperationType.LessThanOrEquals, (property, value) => Expression.LessThanOrEqual(property, value) }
        };
        }

        private static Expression ApplyStringMethodIfString(Expression expression, string methodName)
        {
            if (expression.Type == typeof(string))
            {
                var method = typeof(string).GetMethod(methodName, Type.EmptyTypes);
                if (method != null)
                {
                    return Expression.Call(expression, method);
                }
            }
            return expression;
        }
    }
}
