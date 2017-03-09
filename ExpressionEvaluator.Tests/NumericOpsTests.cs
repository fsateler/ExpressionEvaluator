namespace ExpressionEvaluator.Tests
{
    using Xunit;
    using static Helpers;
    public class NumericOpsTests
    {
        [Theory]
        // X = y + z
        [InlineData(2, 1, 1)]
        [InlineData(0, 1, -1)]
        [InlineData(0, -1, 1)]
        public void EvaluatorAddsCorrectly(int expectedResult, int a, int b)
        {
            TestEval((int i) => i + a, b);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(-2, 1, -1)]
        [InlineData(2, -1, 1)]
        public void EvaluatorSubsCorrectly(int expectedResult, int a, int b)
        {
            TestEval((int i) => i - a, b);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EvaluatorNegatesBool(bool val)
        {
            TestEval(() => !val);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void EvaluatorNegates(int val)
        {
            TestEval(() => -val);
        }
    }
}
