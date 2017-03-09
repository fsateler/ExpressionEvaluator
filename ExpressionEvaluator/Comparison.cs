using System;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    static partial class ExpressionEvaluator
    {
        private static bool EvalEqualExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.Equal);

            return object.Equals(left, right);
        }

        private static bool EvalNotEqualExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.NotEqual);

            return !object.Equals(left, right);
        }

        private static bool EvalLessThanOrEqualExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.LessThanOrEqual);
            if (left == null || right == null) {
                return false;
            }
            return ((IComparable)left).CompareTo(right) <= 0;
        }

        private static bool EvalLessThanExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.LessThan);
            if (left == null || right == null) {
                return false;
            }
            return ((IComparable)left).CompareTo(right) < 0;
        }

        private static bool EvalGreaterThanOrEqualExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.GreaterThanOrEqual);
            if (left == null || right == null) {
                return false;
            }
            return ((IComparable)left).CompareTo(right) >= 0;
        }

        private static bool EvalGreaterThanExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.GreaterThan);
            if (left == null || right == null) {
                return false;
            }
            return ((IComparable)left).CompareTo(right) > 0;
        }
    }
}
