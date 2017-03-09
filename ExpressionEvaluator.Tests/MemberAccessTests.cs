
namespace ExpressionEvaluator.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Helpers;

    public class MemberAccessTests
    {
        [Fact]
        public void EvaluatorThrowsNullExceptionOnNullMemberAccess()
        {
            TestEval(s => s.Length, (string)null);
            TestEval(s => s.Item1.Length, new Tuple<string>(null));
            TestEval(s => s.Name, (Type)null);
            TestEval(s => s.Item1.Name, new Tuple<Type>(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("asdf")]
        public void EvaluatorCanAccessProperties(string str)
        {
            TestEval(() => str.Length);
            TestEval(s => s.Length, str);
            TestEval(s => s.Item1.Length, new Tuple<string>(str));
        }

        [Theory]
        [InlineData("")]
        [InlineData("asdf")]
        public void EvaluatorCanAccessFields(string str)
        {
            var item = new FieldClass<string>(str);
            TestEval(() => item.Field);
        }

        [Fact]
        public void EvaluatorCanAccessArrays()
        {
            var arr = new[] { 1, 2, 3 };
            TestEval(() => arr[0]);
            var arr2 = new[] { new[] { 1, 2, 3 } };
            TestEval(() => arr2[0][0]);

            var arr3 = new[,] { { 1, 2, 3 }, { 1, 2, 3 } };
            TestEval(() => arr3[0, 0]);
        }

        [Fact]
        public void EvaluatorCanAccessIndexes()
        {
            var list = new List<int> { 0 };
            TestEval(() => list[0]);
        }

        private class FieldClass<T> : IEquatable<FieldClass<T>>
        {
            public readonly T Field;

            public FieldClass(T field)
            {
                Field = field;
            }

            public bool Equals(FieldClass<T> other)
            {
                return other != null && EqualityComparer<T>.Default.Equals(Field, other.Field);
            }

            public override bool Equals(object obj) => Equals(obj as FieldClass<T>);
            public override int GetHashCode() => Field?.GetHashCode() ?? 42;
        }
    }
}
