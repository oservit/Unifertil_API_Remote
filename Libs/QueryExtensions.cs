using Libs;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Libs
{
    public static class QueryExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            OrderByRequest? orderBy)
        {
            if (orderBy?.SortProperty == null || orderBy.SortDirection == null)
                return query;

            var propertyInfo = typeof(T).GetProperty(orderBy.SortProperty, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new ArgumentException($"A propriedade '{orderBy.SortProperty}' não foi encontrada em '{typeof(T).Name}'.");

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            return orderBy.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderByDescending(lambda)
                : query.OrderBy(lambda);
        }
    }
}