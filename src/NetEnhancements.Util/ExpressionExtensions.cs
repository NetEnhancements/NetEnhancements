using System.Linq.Expressions;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for expressions.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns the member name the <paramref name="expression"/> points to.
        /// </summary>
        /// <exception cref="ArgumentException">When the <paramref name="expression"/> doesn't point to a member.</exception>
        public static string GetMemberName(this Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.MemberAccess => ((MemberExpression)expression).Member.Name,
                ExpressionType.Convert => GetMemberName(((UnaryExpression)expression).Operand),
                _ => throw new ArgumentException($"Cannot get the name of a {expression.NodeType} expression", nameof(expression))
            };
        }

        /// <summary>
        /// Returns the value of the member the <paramref name="propertySelector"/> points to, or throws when that doesn't.
        /// </summary>
        /// <exception cref="ArgumentException">When the <paramref name="propertySelector"/> doesn't point to a callable member.</exception>
        public static (string MemberName, TValue? Value) GetMemberValue<T, TValue>(T instance, Expression<Func<T, TValue>> propertySelector)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (propertySelector.NodeType != ExpressionType.Lambda || propertySelector.Body is not MemberExpression memberExpression)
            {
                throw new ArgumentException("Expression does not point to a property name", nameof(propertySelector));
            }

            var compiledExpression = propertySelector.Compile();

            TValue propertyValue = compiledExpression.Invoke(instance);

            return (memberExpression.Member.Name, propertyValue);
        }
    }
}
