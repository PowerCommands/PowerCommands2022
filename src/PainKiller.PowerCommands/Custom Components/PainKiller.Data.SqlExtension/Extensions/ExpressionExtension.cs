using System;
using System.Linq.Expressions;

namespace PainKiller.Data.SqlExtension.Extensions
{
    public static class ExpressionExtension
    {
        
        public static string ToSqlOperator(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.And:
                    return "AND";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.IsFalse:
                    return "!=";
                case ExpressionType.IsTrue:
                    return "=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Not:
                    return "!=";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.Or:
                    return "OR";
                default:
                    return "";
            }
        }
        public static string GetSqlValue(this Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var objectValue = getterLambda.Compile().Invoke();
            return objectValue.ToSqlFormattedValue();
        }

        public static string GetName(this Expression expression, bool recursiveCall = false)
        {
            var member = expression as MemberExpression;
            if (member == null && !recursiveCall)
            {
                var unaryMember = expression as UnaryExpression;
                
                return unaryMember?.Operand.GetName(recursiveCall: true);
            }
            return member?.Member.Name;
        }
        public static string ToSqlConditionalClause<T>(this Expression<Func<T, bool>> expression)
        {
            var bodyExpression = (BinaryExpression)expression.Body;

            var name = bodyExpression.Left.GetName();
            var sqlOperator = expression.Body.NodeType.ToSqlOperator();
            var value = bodyExpression.Right.GetSqlValue();
            
            return $"{name} {sqlOperator} {value}";
        }
    }
}