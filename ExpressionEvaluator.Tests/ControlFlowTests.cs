
namespace ExpressionEvaluator.Tests
{
    using Xunit;
    using static Helpers;
    public class ControlFlowTests
    {
#pragma warning disable S2583 // Conditionally executed blocks should be reachable
        [Fact]
        public void EvaluatorGetsConditionals()
        {
            TestEval(() => false ? 1 : 0);
            TestEval(() => true ? 1 : 0);
            int? i = null;
            TestEval(() => i ?? 1);
            i = 2;
            TestEval(() => i ?? 1);
        }

        [Fact]
        public void EvaluatorConditionalsShortCircuits()
        {
            TestEval(() => false ? Unreachable<int>() : 0);
            TestEval(() => true ? 1 : Unreachable<int>());
            int? i = 2;
            TestEval(() => i ?? Unreachable<int>());
            TestEval(() => true || Unreachable<bool>());
            TestEval(() => false && Unreachable<bool>());
        }
#pragma warning restore S2583 // Conditionally executed blocks should be reachable

        private static T Unreachable<T>()
        {
            Assert.True(false, "Unreachable code was reached");
            return default(T);
        }
    }
}
