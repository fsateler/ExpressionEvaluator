namespace ExpressionEvaluator.Tests
{
    using Xunit;
    using static Helpers;

    public class MethodCallTests
    {

        [Theory]
        [InlineData("")]
        [InlineData("asdf")]
        public void EvaluatorCanCallStaticMethods(string str)
        {
            TestEval(s => string.IsNullOrEmpty(s), str);
        }

        [Theory]
        [InlineData("")]
        [InlineData("asdf")]
        public void EvaluatorCanCallInstanceMethods(string str)
        {
            TestEval(s => s.GetHashCode(), str);
        }
    }
}
