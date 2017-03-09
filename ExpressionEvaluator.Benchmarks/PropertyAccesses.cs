using System;
using System.Linq;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;

namespace ExpressionEvaluator.Benchmarks
{
    public class PropertyAccesses
    {
        readonly Expression<Func<Tuple<int>, int>> expr;
        readonly Func<Tuple<int>, int> func;

        readonly Tuple<int> tuple = Tuple.Create(1);

        public PropertyAccesses()
        {
            expr = i => i.Item1;
            func = expr.Compile();
        }

        [Benchmark]
        public void CompileEveryTime()
        {
            expr.Compile().Invoke(tuple);
        }

        [Benchmark]
        public void CompileOnce()
        {
            func.Invoke(tuple);
        }

        [Benchmark]
        public void Evaluate()
        {
            expr.Evaluate(tuple);
        }
    }
}
