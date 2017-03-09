using System;
using BenchmarkDotNet.Running;
namespace ExpressionEvaluator.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<PropertyAccesses>();
        }
    }
}