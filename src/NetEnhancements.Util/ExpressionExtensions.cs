using System.Linq.Expressions;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for expressions.
    /// </summary>
    public static class ExpressionExtensions
    {
        public static string GetMemberName(this Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.MemberAccess => ((MemberExpression)expression).Member.Name,
                ExpressionType.Convert => GetMemberName(((UnaryExpression)expression).Operand),
                _ => throw new NotSupportedException($"Cannot get the name of a {expression.NodeType} expression")
            };
        }

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
