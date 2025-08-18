using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Libs
{
    public static class FilterBuilder
    {
        public static IQueryable<T> BuildFilter<T>(IQueryable<T> query, FilterRequest filterRequest)
        {
            var operations = FilterOperations.GetOperations();
            var parameter = Expression.Parameter(typeof(T), "x");
            var filterExpression = BuildFilterExpression(filterRequest, operations, parameter);

            var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
            return query.Where(lambda);
        }

        private static Expression BuildFilterExpression(
            FilterRequest filter_request,
            Dictionary<OperationType, Func<Expression, Expression, Expression>> operations,
            ParameterExpression parameter)
        {
            Expression? expression = null;

            foreach (var filter in filter_request.Filters)
            {
                var property = Expression.Property(parameter, filter.PropertyName);
                var converted_value = ConvertFilterValue(filter.Value, property.Type, filter.Operation);

                Expression value_expression = converted_value == null && property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Expression.Constant(null, property.Type)
                    : Expression.Constant(converted_value, property.Type);

                if (operations.TryGetValue(filter.Operation, out var operation))
                {
                    var current_expression = operation(property, value_expression);

                    expression = expression == null
                        ? current_expression
                        : (filter_request.Connector.ToLower() == "and"
                            ? Expression.AndAlso(expression, current_expression)
                            : Expression.OrElse(expression, current_expression));
                }
            }

            if (expression != null && expression.Type != typeof(bool))
                throw new InvalidOperationException("The filter expression does not return a boolean.");

            return expression ?? Expression.Constant(false);
        }

        public static bool ValidateFilter(FilterRequest? filterRequest)
        {
            if(!string.IsNullOrEmpty(filterRequest?.Connector))
            {
                if (filterRequest.Filters.Count != 0)
                    return true;
                else
                    throw new Exception("Filtro Inválido!");
            }
            return false;
        }

        private static object? ConvertFilterValue(object? filterValue, Type targetType, OperationType operation)
        {
            if (filterValue == null)
                return Nullable.GetUnderlyingType(targetType) != null ? null : Activator.CreateInstance(targetType);

            var valueAsString = filterValue.ToString();

            if (operation == OperationType.Contains)
                return valueAsString;

            var nonNullableTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (nonNullableTargetType.IsAssignableFrom(filterValue.GetType()))
                return filterValue;

            switch (nonNullableTargetType)
            {
                case Type t when t == typeof(string):
                    return valueAsString ?? null;
                case Type t when t == typeof(DateTime):
                    return string.IsNullOrEmpty(valueAsString) ? (DateTime?)null : DateTime.Parse(valueAsString);
                case Type t when t.IsEnum:
                    return string.IsNullOrEmpty(valueAsString) ? null : Enum.Parse(t, valueAsString);
                case Type t when t == typeof(long):
                    return string.IsNullOrEmpty(valueAsString) ? (long?)null : long.Parse(valueAsString);
                case Type t when t == typeof(int):
                    return string.IsNullOrEmpty(valueAsString) ? (int?)null : int.Parse(valueAsString);
                case Type t when t == typeof(double):
                    return string.IsNullOrEmpty(valueAsString) ? (double?)null : double.Parse(valueAsString);
                case Type t when t == typeof(bool):
                    return string.IsNullOrEmpty(valueAsString) ? (bool?)null : bool.Parse(valueAsString);
                default:
                    return filterValue == null ? null : Convert.ChangeType(filterValue, nonNullableTargetType);
            }
        }
    }
}
