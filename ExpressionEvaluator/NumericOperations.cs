using System;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    static partial class ExpressionEvaluator
    {
        private static object EvalSubtractExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.Subtract, ExpressionType.SubtractChecked);

            switch (Type.GetTypeCode(expr.Type)) {
                case TypeCode.Byte:
                    return (byte)left - (byte)right;
                case TypeCode.Char:
                    return (char)left - (char)right;
                case TypeCode.Decimal:
                    return (decimal)left - (decimal)right;
                case TypeCode.Double:
                    return (double)left - (double)right;
                case TypeCode.Int16:
                    return (Int16)left - (Int16)right;
                case TypeCode.Int32:
                    return (Int32)left - (Int32)right;
                case TypeCode.Int64:
                    return (Int64)left - (Int64)right;
                case TypeCode.SByte:
                    return (sbyte)left - (sbyte)right;
                case TypeCode.Single:
                    return (float)left - (float)right;
                case TypeCode.UInt16:
                    return (UInt16)left - (UInt16)right;
                case TypeCode.UInt32:
                    return (UInt32)left - (UInt32)right;
                case TypeCode.UInt64:
                    return (UInt64)left - (UInt64)right;
                case TypeCode.String:
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException("Invalid type");
            }
        }

        private static object EvalAddExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.Add, ExpressionType.AddChecked);

            switch (Type.GetTypeCode(expr.Type)) {
                case TypeCode.Byte:
                    return (byte)left + (byte)right;
                case TypeCode.Char:
                    return (char)left + (char)right;
                case TypeCode.Decimal:
                    return (decimal)left + (decimal)right;
                case TypeCode.Double:
                    return (double)left + (double)right;
                case TypeCode.Int16:
                    return (Int16)left + (Int16)right;
                case TypeCode.Int32:
                    return (Int32)left + (Int32)right;
                case TypeCode.Int64:
                    return (Int64)left + (Int64)right;
                case TypeCode.SByte:
                    return (sbyte)left + (sbyte)right;
                case TypeCode.Single:
                    return (float)left + (float)right;
                case TypeCode.UInt16:
                    return (UInt16)left + (UInt16)right;
                case TypeCode.UInt32:
                    return (UInt32)left + (UInt32)right;
                case TypeCode.UInt64:
                    return (UInt64)left + (UInt64)right;
                case TypeCode.String:
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException("Invalid type");
            }
        }

        private static object EvalMultiplyExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.Multiply, ExpressionType.MultiplyChecked);

            switch (Type.GetTypeCode(expr.Type)) {
                case TypeCode.Byte:
                    return (byte)left * (byte)right;
                case TypeCode.Char:
                    return (char)left * (char)right;
                case TypeCode.Decimal:
                    return (decimal)left * (decimal)right;
                case TypeCode.Double:
                    return (double)left * (double)right;
                case TypeCode.Int16:
                    return (Int16)left * (Int16)right;
                case TypeCode.Int32:
                    return (Int32)left * (Int32)right;
                case TypeCode.Int64:
                    return (Int64)left * (Int64)right;
                case TypeCode.SByte:
                    return (sbyte)left * (sbyte)right;
                case TypeCode.Single:
                    return (float)left * (float)right;
                case TypeCode.UInt16:
                    return (UInt16)left * (UInt16)right;
                case TypeCode.UInt32:
                    return (UInt32)left * (UInt32)right;
                case TypeCode.UInt64:
                    return (UInt64)left * (UInt64)right;
                case TypeCode.String:
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException("Invalid type");
            }
        }

        private static object EvalDivideExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            if (expr.NodeType != ExpressionType.Divide) {
                throw new ArgumentException("Expression must be divide type", nameof(expr));
            }

            switch (Type.GetTypeCode(expr.Type)) {
                case TypeCode.Byte:
                    return (byte)left / (byte)right;
                case TypeCode.Char:
                    return (char)left / (char)right;
                case TypeCode.Decimal:
                    return (decimal)left / (decimal)right;
                case TypeCode.Double:
                    return (double)left / (double)right;
                case TypeCode.Int16:
                    return (Int16)left / (Int16)right;
                case TypeCode.Int32:
                    return (Int32)left / (Int32)right;
                case TypeCode.Int64:
                    return (Int64)left / (Int64)right;
                case TypeCode.SByte:
                    return (sbyte)left / (sbyte)right;
                case TypeCode.Single:
                    return (float)left / (float)right;
                case TypeCode.UInt16:
                    return (UInt16)left / (UInt16)right;
                case TypeCode.UInt32:
                    return (UInt32)left / (UInt32)right;
                case TypeCode.UInt64:
                    return (UInt64)left / (UInt64)right;
                case TypeCode.String:
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException("Invalid type");
            }
        }

        private static object EvalModuloExpression(BinaryExpression expr, object left, object right)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            AssertType(expr, ExpressionType.Modulo);

            switch (Type.GetTypeCode(expr.Type)) {
                case TypeCode.Byte:
                    return (byte)left % (byte)right;
                case TypeCode.Char:
                    return (char)left % (char)right;
                case TypeCode.Decimal:
                    return (decimal)left % (decimal)right;
                case TypeCode.Double:
                    return (double)left % (double)right;
                case TypeCode.Int16:
                    return (Int16)left % (Int16)right;
                case TypeCode.Int32:
                    return (Int32)left % (Int32)right;
                case TypeCode.Int64:
                    return (Int64)left % (Int64)right;
                case TypeCode.SByte:
                    return (sbyte)left % (sbyte)right;
                case TypeCode.Single:
                    return (float)left % (float)right;
                case TypeCode.UInt16:
                    return (UInt16)left % (UInt16)right;
                case TypeCode.UInt32:
                    return (UInt32)left % (UInt32)right;
                case TypeCode.UInt64:
                    return (UInt64)left % (UInt64)right;
                case TypeCode.String:
                case TypeCode.DateTime:
                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.Empty:
                default:
                    throw new ArgumentException("Invalid type");
            }
        }

        private static object NegateNumber(object num)
        {

            switch (Type.GetTypeCode(num.GetType())) {
                case TypeCode.Byte:
                    return -(byte)num;
                case TypeCode.Char:
                    return -(char)num;
                case TypeCode.Decimal:
                    return -(decimal)num;
                case TypeCode.Double:
                    return -(double)num;
                case TypeCode.Int16:
                    return -(Int16)num;
                case TypeCode.Int32:
                    return -(Int32)num;
                case TypeCode.Int64:
                    return -(Int64)num;
                case TypeCode.SByte:
                    return -(sbyte)num;
                case TypeCode.Single:
                    return -(float)num;
                case TypeCode.UInt16:
                    return -(UInt16)num;
                case TypeCode.UInt32:
                    return -(UInt32)num;
                case TypeCode.UInt64:
                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                case TypeCode.Empty:
                case TypeCode.String:
                default:
                    throw new ArgumentException("Invalid type", nameof(num));
            }
        }
    }
}
