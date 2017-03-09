namespace ExpressionEvaluator.Tests
{
    using Xunit;
    using static Helpers;
    public class ComparisonTests
    {

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        public void EvaluatorComparesLessThanOrEqual(int a, int b)
        {
            TestEval(() => a <= b);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        public void EvaluatorComparesLessThan(int a, int b)
        {
            TestEval(() => a < b);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        public void EvaluatorComparesGreaterThanOrEqual(int a, int b)
        {
            TestEval(() => a >= b);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        public void EvaluatorComparesGreaterThan(int a, int b)
        {
            TestEval(() => a > b);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        public void EvaluatorComparesEqual(int? a, int? b)
        {
            TestEval(() => a == b);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MinValue)]
        [InlineData(1, null)]
        [InlineData(null, 1)]
        [InlineData(null, null)]
        public void EvaluatorComparesNotEqual(int a, int b)
        {
            TestEval(() => a != b);
        }
    }
}
