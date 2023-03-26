using System;
using System.Linq.Expressions;

namespace NetEnhancements.Util
{
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
    }
}
