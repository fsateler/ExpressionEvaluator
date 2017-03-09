using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    public static partial class ExpressionEvaluator
    {
        public static object Evaluate(this LambdaExpression expr, params object[] parameters)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            if (parameters == null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Length != expr.Parameters.Count) {
                throw new ArgumentException("The number of parameters is not the same as the number of arguments");
            }
            var paramExprs = expr.Parameters.Zip(parameters, (pExpr, pObj) => new { Orig = pExpr, Res = Expression.Constant(pObj, pExpr.Type) });
            var boundBody = expr.Body;
            foreach (var param in paramExprs) {
                boundBody = boundBody.ReplaceExpressions(param.Orig, param.Res);
            }
            return GetValue(boundBody);
        }

        public static T Evaluate<T>(this Expression<Func<T>> expr)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            object val = GetValue(expr.Body);
            return (T)val;
        }

        public static TRes Evaluate<TIn, TRes>(this Expression<Func<TIn, TRes>> expr, TIn arg)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            var argExpression = Expression.Constant(arg, typeof(TIn));
            var boundBody = expr.Body.ReplaceExpressions(expr.Parameters[0], argExpression);
            var boundExpr = Expression.Lambda<Func<TRes>>(boundBody);
            return Evaluate(boundExpr);
        }

        public static TRes Evaluate<TIn1, TIn2, TRes>(this Expression<Func<TIn1, TIn2, TRes>> expr, TIn1 arg1, TIn2 arg2)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            var arg1Expression = Expression.Constant(arg1, typeof(TIn1));
            var arg2Expression = Expression.Constant(arg2, typeof(TIn2));
            var boundBody = expr.Body.ReplaceExpressions(expr.Parameters[0], arg1Expression).ReplaceExpressions(expr.Parameters[1], arg2Expression);

            var boundExpr = Expression.Lambda<Func<TRes>>(boundBody);
            return Evaluate(boundExpr);
        }

        private static void AssertType(Expression expr, ExpressionType expected)
        {
            if (expr.NodeType != expected) {
                throw new ArgumentException($"Expression must be {expected}", nameof(expr));
            }
        }

        private static void AssertType(Expression expr, ExpressionType expected1, ExpressionType expected2)
        {
            if (expr.NodeType != expected1 && expr.NodeType != expected2) {
                throw new ArgumentException($"Expression must be {expected1} or {expected2}", nameof(expr));
            }
        }
    }
}
