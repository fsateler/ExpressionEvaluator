using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionEvaluator
{
    static partial class ExpressionEvaluator
    {
        private static object GetValue(Expression expr)
        {
            if (expr == null) {
                throw new ArgumentNullException(nameof(expr));
            }
            switch (expr) {
            case MemberExpression member:
                return VisitImpl(member);
            case ConstantExpression constant:
                return VisitImpl(constant);
            case MethodCallExpression call:
                return VisitImpl(call);
            case UnaryExpression unary:
                return VisitImpl(unary);
            case BinaryExpression binary:
                return VisitImpl(binary);
            case ConditionalExpression conditional:
                return VisitImpl(conditional);
            case NewExpression newExpr:
                return VisitImpl(newExpr);
            case MemberInitExpression memberInit:
                return VisitImpl(memberInit);
            case ListInitExpression listInit:
                return VisitImpl(listInit);
            default:
                throw new ArgumentException(String.Format("Unknown node type {0}", expr.NodeType));
            }
        }

        private static object VisitImpl(ListInitExpression listInit)
        {
            var obj = VisitImpl(listInit.NewExpression);
            InvokeInitializers(obj, listInit.Initializers);
            return obj;
        }

        private static void InvokeInitializers(object obj, ReadOnlyCollection<ElementInit> initializers)
        {
            foreach (var initializer in initializers) {
                var parameters = initializer.Arguments.Select(GetValue).ToArray();
                initializer.AddMethod.Invoke(obj, parameters);
            }
        }

        private static object VisitImpl(MemberInitExpression memberInit)
        {
            var obj = VisitImpl(memberInit.NewExpression);
            var bindings = memberInit.Bindings;
            SetBindings(obj, bindings);
            return obj;
        }

        private static void SetBindings(object obj, ReadOnlyCollection<MemberBinding> bindings)
        {
            foreach (var binding in bindings) {
                MemberInfo member = binding.Member;
                switch (binding) {
                case MemberAssignment assign:
                    AssignMember(obj, member, assign);
                    break;
                case MemberMemberBinding memberMember:
                    var memberValue = GetMemberValue(obj, member);
                    SetBindings(memberValue, memberMember.Bindings);
                    break;
                case MemberListBinding memberList:
                    var listValue = GetMemberValue(obj, member);
                    InvokeInitializers(listValue, memberList.Initializers);
                    break;
                default:
                    throw new ArgumentException($"Cannot handle {binding.BindingType} binding");
                }
            }
        }

        private static void AssignMember(object obj, MemberInfo member, MemberAssignment assign)
        {
            switch (member.MemberType) {
            case MemberTypes.Field:
                ((FieldInfo)assign.Member).SetValue(obj, GetValue(assign.Expression));
                break;
            case MemberTypes.Property:
                ((PropertyInfo)assign.Member).SetValue(obj, GetValue(assign.Expression));
                break;
            default:
                throw new ArgumentException($"Cannot assign member type {member.MemberType}");
            }
        }

        private static object VisitImpl(NewExpression newExpr)
        {
            var constructor = newExpr.Constructor;

            if (constructor != null) {
                var parameters = newExpr.Arguments.Select(GetValue).ToArray();

                return constructor.Invoke(parameters);
            }
            else {
                return Activator.CreateInstance(newExpr.Type);
            }
        }

        static object VisitImpl(ConditionalExpression expr)
        {
            bool test = (bool)GetValue(expr.Test);
            if (test) {
                return GetValue(expr.IfTrue);
            }
            else {
                return GetValue(expr.IfFalse);
            }
        }

        static object VisitImpl(MemberExpression expr)
        {
            object obj = GetValue(expr.Expression);
            return GetMemberValue(obj, expr.Member);
        }

        private static object GetMemberValue(object obj, MemberInfo member)
        {
            if (obj == null) {
#pragma warning disable S112 // General exceptions should never be thrown
                throw new NullReferenceException();
#pragma warning restore S112 // General exceptions should never be thrown
            }
            object value;
            switch (member) {
            case FieldInfo field:
                value = field.GetValue(obj);
                break;
            case PropertyInfo prop:
                value = prop.GetValue(obj);
                break;
            default:
                throw new ArgumentException(string.Format("Invalid member info type {0}", member.MemberType));
            }

            return value;
        }

        static object VisitImpl(MethodCallExpression expr)
        {
            object instance = null;
            if (expr.Object != null) {
                instance = GetValue(expr.Object);
            }
            object[] parameters = expr.Arguments.Select(GetValue).ToArray();
            return expr.Method.Invoke(instance, parameters);
        }

        static object VisitImpl(UnaryExpression expr)
        {
            object instance = GetValue(expr.Operand);
            if (instance == null) {
                return null;
            }

            switch (expr.NodeType) {
                case ExpressionType.Convert:
                    return ConvertObject(instance, expr.Type, expr.Operand.Type, false);
                case ExpressionType.TypeAs:
                    return ConvertObject(instance, expr.Type, expr.Operand.Type, true);
                case ExpressionType.ArrayLength:
                    return ((Array)instance).Length;
                case ExpressionType.Negate:
                    return NegateNumber(instance);
                case ExpressionType.Not:
                    return !(bool)instance;
                case ExpressionType.UnaryPlus:
                    return instance;
                case ExpressionType.ConvertChecked:
                case ExpressionType.NegateChecked:
                case ExpressionType.Quote:
                default:
                    throw new ArgumentException(String.Format("Invalid unary node type {0}", expr.NodeType));
            }
        }

        static object VisitImpl(BinaryExpression expr)
        {
            object left = GetValue(expr.Left);
            Func<object> right = () => GetValue(expr.Right);
            switch (expr.NodeType) {
                // misc
                case ExpressionType.ArrayIndex:
                    if (left == null) {
                        return null;
                    }
                    Array arr = (Array)left;
                    if (arr.Rank == 1) {
                        return arr.GetValue((int)right());
                    }
                    throw new InvalidOperationException("Cannot get index of multidimensional array");
                case ExpressionType.Coalesce:
                    return left ?? right();
                // Conditionals
                case ExpressionType.AndAlso:
                    return (bool)left && (bool)right();
                case ExpressionType.OrElse:
                    return (bool)left || (bool)right();
                // Unimplemented
                case ExpressionType.Add:
                    unchecked {
                        return EvalAddExpression(expr, left, right());
                    };
                case ExpressionType.AddChecked:
                    checked {
                        return EvalAddExpression(expr, left, right());
                    }
                case ExpressionType.Subtract:
                    unchecked {
                        return EvalSubtractExpression(expr, left, right());
                    }
                case ExpressionType.SubtractChecked:
                    checked {
                        return EvalSubtractExpression(expr, left, right());
                    }
                case ExpressionType.Multiply:
                    unchecked {
                        return EvalMultiplyExpression(expr, left, right());
                    }
                case ExpressionType.MultiplyChecked:
                    checked {
                        return EvalMultiplyExpression(expr, left, right());
                    }
                case ExpressionType.Divide:
                    return EvalDivideExpression(expr, left, right());
                case ExpressionType.Modulo:
                    return EvalModuloExpression(expr, left, right());
                case ExpressionType.Equal:
                    return EvalEqualExpression(expr, left, right());
                case ExpressionType.NotEqual:
                    return EvalNotEqualExpression(expr, left, right());
                case ExpressionType.GreaterThanOrEqual:
                    return EvalGreaterThanOrEqualExpression(expr, left, right());
                case ExpressionType.GreaterThan:
                    return EvalGreaterThanExpression(expr, left, right());
                case ExpressionType.LessThan:
                    return EvalLessThanExpression(expr, left, right());
                case ExpressionType.LessThanOrEqual:
                    return EvalLessThanOrEqualExpression(expr, left, right());
                case ExpressionType.Power:
                case ExpressionType.And:
                case ExpressionType.Or:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                default:
                    throw new ArgumentException(String.Format("Invalid binary node type {0}", expr.NodeType));
            }
        }

        static object VisitImpl(ConstantExpression expr)
        {
            return expr.Value;
        }
    }
}
