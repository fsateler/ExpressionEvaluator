namespace ExpressionEvaluator.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Helpers;

    public class CreateObjectTests
    {
        [Fact]
        public void EvaluatorCanCreateObjects()
        {
            TestEval(() => new KeyValuePair<int, decimal>());
            TestEval(i => new { i }, 2);
            TestEval(i => new Tuple<int>(i), 2);
            TestEval(i => new Record<int> { Item = i }, 2);
            TestEval((i, j) => new List<int> { i, j }, 1, 2);
            TestEval(i => new Nested<int> { Record = { Item = i } }, 1);
            TestEval(i => new Tuple<List<int>>(new List<int>()) {
                Item1 = {
                    1,
                    2,
                    3
                }
            }, 1, AssertTupleListEqual);

            void AssertTupleListEqual(Tuple<List<int>> a, Tuple<List<int>> b)
            {
                Assert.Equal(string.Join(", ", a.Item1), string.Join(", ", b.Item1));
            }
        }

        private class Nested<T> : IEquatable<Nested<T>>
        {
            public Record<T> Record { get; } = new Record<T>();

            public bool Equals(Nested<T> other)
            {
                return other != null && Record.Equals(other.Record);
            }
            public override bool Equals(object obj) => Equals(obj as Nested<T>);
            public override int GetHashCode() => Record.GetHashCode();
        }

        private class Record<T> : IEquatable<Record<T>>
        {
            public T Item { get; set; } = default(T);

            public bool Equals(Record<T> other)
            {
                return other != null && EqualityComparer<T>.Default.Equals(Item, other.Item);
            }

            public override bool Equals(object obj) => Equals(obj as Record<T>);
            public override int GetHashCode() => Item?.GetHashCode() ?? 42;
        }
    }
}
