using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ShoesStore.MyExtensions
{
    public static partial class MyExtensions
    {
        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>
        (this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            bool descending)
        {
            return descending
                ? source.OrderByDescending(keySelector)
                : source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>
        (this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            bool descending)
        {
            return descending
                ? source.OrderByDescending(keySelector)
                : source.OrderBy(keySelector);
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null) throw new ArgumentNullException("source");
// DataSource control passes the sort parameter with a direction
// if the direction is descending
            var descIndex = propertyName.IndexOf(" DESC");
            if (descIndex >= 0) propertyName = propertyName.Substring(0, descIndex).Trim();
            if (string.IsNullOrEmpty(propertyName)) return source;
            var parameter = Expression.Parameter(source.ElementType, string.Empty);
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);
            var methodName = descIndex < 0 ? "OrderBy" : "OrderByDescending";
            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                new[] {source.ElementType, property.Type},
                source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}