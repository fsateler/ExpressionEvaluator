using System;
using System.Linq;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;

namespace ExpressionEvaluator.Benchmarks
{
    public class MathPow
    {
        readonly Expression<Func<double, double>> expr;
        readonly Func<double, double> func;

        public MathPow()
        {
            expr = i => Math.Pow(Math.E, i);
            func = expr.Compile();
        }

        [Benchmark]
        public void CompileEveryTime()
        {
            expr.Compile().Invoke(5.2);
        }

        [Benchmark]
        public void CompileOnce()
        {
            func.Invoke(5.2);
        }

        [Benchmark]
        public void Evaluate()
        {
            expr.Evaluate(5.2);
        }
    }
}
