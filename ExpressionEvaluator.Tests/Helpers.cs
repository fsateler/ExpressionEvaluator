
namespace ExpressionEvaluator.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;

    public static class Helpers
    {

        public static void TestEval<T>(Expression<Func<T>> expr, Action<T, T> assertEqual = null)
        {
            var func = expr.Compile();
            TestCompeting(() => func(), () => ExpressionEvaluator.Evaluate(expr), assertEqual);
        }

        public static void TestEval<T1, T2>(Expression<Func<T1, T2>> expr, T1 arg1, Action<T2, T2> assertEqual = null)
        {
            var func = expr.Compile();
            TestCompeting(() => func(arg1), () => ExpressionEvaluator.Evaluate(expr, arg1), assertEqual);
        }

        public static void TestEval<T1, T2, T3>(Expression<Func<T1, T2, T3>> expr, T1 arg1, T2 arg2, Action<T3, T3> assertEqual = null)
        {
            var func = expr.Compile();
            TestCompeting(() => func(arg1, arg2), () => ExpressionEvaluator.Evaluate(expr, arg1, arg2), assertEqual);
        }

        private static void TestCompeting<T>(Func<T> expectedFunc, Func<T> actualFunc, Action<T, T> assertEqual)
        {
            var expected = GetResult(expectedFunc);
            var actual = GetResult(actualFunc);
            AssertEqual(expected, actual, assertEqual);
        }

        private static Task<T> GetResult<T>(Func<T> func)
        {
            T res;
            try {
                res = func();
            }
            catch (Exception e) {
                return Task.FromException<T>(e);
            }
            return Task.FromResult(res);
        }

        private static void AssertEqual<T>(Task<T> expected, Task<T> actual, Action<T, T> assertEqual)
        {
            assertEqual = assertEqual ?? Assert.Equal;
            Assert.True(expected.IsCompleted);
            Assert.True(actual.IsCompleted);
            Assert.False(expected.IsCanceled);
            Assert.False(actual.IsCanceled);
            if (expected.Status == TaskStatus.RanToCompletion) {
                assertEqual(expected.Result, actual.Result);
            }
            else {
                var expectedException = expected.Exception.InnerExceptions.Single();
                var actualException = actual.Exception.InnerExceptions.Single();
                Assert.IsAssignableFrom(FirstPublicType(expectedException.GetType()), actualException);
            }
        }

        private static Type FirstPublicType(Type root)
        {
            Type t = root;
            while (true) {
                if (t.GetTypeInfo().IsPublic) {
                    return t;
                }
                t = t.GetTypeInfo().BaseType;
            }
        }
    }
}
