
namespace ExpressionEvaluator.Tests
{
    using System;
    using Xunit;
    using static Helpers;
    public class CastTests
    {
        [Fact]
#pragma warning disable CS0458 // The result of the expression is always 'null'
        public void EvaluatorCastsShortsCorrectly()
        {
            EvaluatorCastsCorrectly<short, int>(1);
            short i = 1;
            TestEval(() => (decimal)i);
            TestEval(() => (decimal?)i);
            TestEval(() => i as decimal?);
            TestEval(() => (double)i);
            TestEval(() => (double?)i);
            TestEval(() => i as double?);
            TestEval(() => (int)i);
            TestEval(() => (int?)i);
            TestEval(() => i as int?);
        }

        [Fact]
        public void EvaluatorCastsIntsCorrectly()
        {
            EvaluatorCastsCorrectly<int, decimal>(1);
            int i = 1;
            TestEval(() => (decimal)i);
            TestEval(() => (decimal?)i);
            TestEval(() => i as decimal?);
            TestEval(() => (double)i);
            TestEval(() => (double?)i);
            TestEval(() => i as double?);
            TestEval(() => (short)i);
            TestEval(() => (short?)i);
            TestEval(() => i as short?);
        }
#pragma warning restore CS0458 // The result of the expression is always 'null'

        [Fact]
        public void CastsToSuperTypeCorrectly()
        {
            var exc = new InvalidOperationException();
            TestEval(() => (Exception)exc);
            TestEval(() => exc as Exception);
        }

        [Fact]
        public void InvalidUpcastCorrectly()
        {
            var exc = new ArgumentException("test exception");
            TestEval(() => (ArgumentNullException)exc);
            TestEval(() => exc as ArgumentNullException);
        }

        private void EvaluatorCastsCorrectly<TOrig, TDifferent>(TOrig i)
            where TOrig : struct, IComparable
            where TDifferent : struct, IComparable
        {
            object v = i;
            TestEval(() => v as TOrig?);
            TestEval(() => (TOrig)v);
            TestEval(() => (IComparable)i);

            // invalid conversions
            TestEval(() => (TDifferent)v);
            TestEval(() => (string)v);
            TestEval(() => v as TDifferent?);
            TestEval(() => v as string);

            object nil = null;
            TestEval(() => (TOrig)nil);
        }
    }
}
