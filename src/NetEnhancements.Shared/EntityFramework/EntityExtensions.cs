using System.ComponentModel;
using System.Linq.Expressions;
using NetEnhancements.Shared.Query;

namespace NetEnhancements.Shared.EntityFramework
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Calls Skip() and Take() to perform pagination as specified in the request.
        /// </summary>
        public static IQueryable<T> Page<T>(this IQueryable<T> source, PagedQuery request)
        {
            return source.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);
        }

        /// <summary>
        /// Applies order.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="query"></param>
        /// <param name="orderer">Translates an order string (e.g. "name") to an expression on the entity (e.g. u => u.Name)</param>
        public static IQueryable<TEntity> ApplyOrder<TEntity>(this IQueryable<TEntity> queryable, DataQuery query, Func<string, Expression<Func<TEntity, object?>>> orderer)
            where TEntity : class
        {
            var order = query.OrderExpressions;

            if (!order.Any())
            {
                return queryable;
            }

            IOrderedQueryable<TEntity>? orderedQueryable = null;

            foreach (var orderExpression in order)
            {
                var expression = orderer(orderExpression.Order);

                orderedQueryable = orderExpression.Direction switch
                {
                    OrderDirection.Ascending => 
                        orderedQueryable == null
                            ? queryable.OrderBy(expression)
                            : orderedQueryable.ThenBy(expression),

                    OrderDirection.Descending => 
                        orderedQueryable == null
                            ? queryable.OrderByDescending(expression)
                            : orderedQueryable.ThenByDescending(expression),

                    _ => throw new InvalidEnumArgumentException(nameof(query), int.MinValue, typeof(OrderDirection)),
                };
            }

            return orderedQueryable!;
        }
    }
}
