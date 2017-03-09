using System;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    static class ExpressionHelpers
    {
        public static TExpr ReplaceExpressions<TExpr>(this TExpr expression, Expression orig, Expression replacement) where TExpr : Expression
        {
            var replacer = new ExpressionReplacer(orig, replacement);
            return replacer.VisitAndConvert(expression, nameof(ReplaceExpressions));
        }

        private class ExpressionReplacer : ExpressionVisitor
        {
            private readonly Expression From;
            private readonly Expression To;

            public ExpressionReplacer(Expression from, Expression to)
            {
                From = from;
                To = to;
            }

            public override Expression Visit(Expression node)
            {
                if (node == From) {
                    return To;
                }
                return base.Visit(node);
            }
        }
    }
}
