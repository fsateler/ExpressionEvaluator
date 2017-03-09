# ExpressionEvaluator
Evaluates LINQ Expressions and provides the result

This library allows you to do the following:

```csharp
Expression<Func<Model, string>> expression = m => m.Property;
string propertyValue = expression.Evaluate(Model);
Debug.Assert(propertyValue == Model.Property);
```

## Why?

Evaluating linq expressions can be done by calling the `Compile` method, but the compilation process is slow.
Therefore one needs to do some sort of caching to avoid compiling the expressions on every call. 
This is annoying and error prone. Much better to quickly evaluate the expression with a small penalty:

```ini
Processor=Intel Core i7-4500U CPU 1.80GHz (Haswell), ProcessorCount=4
Frequency=2338335 Hz, Resolution=427.6547 ns, Timer=TSC
dotnet cli version=1.0.3
  [Host]     : .NET Core 4.6.25009.03, 64bit RyuJIT
  DefaultJob : .NET Core 4.6.25009.03, 64bit RyuJIT
```

#### Computing an expensive math operation (`Math.Pow(5.2)`)

 |           Method |         Mean |         Error |         StdDev |       Median |
 |----------------- |-------------:|--------------:|---------------:|-------------:|
 | CompileEveryTime | 88,627.70 ns | 7,257.2503 ns | 21,284.2588 ns | 76,880.76 ns |
 |      CompileOnce |     50.01 ns |     0.4483 ns |      0.4193 ns |     50.16 ns |
 |         Evaluate |  1,919.45 ns |    34.4638 ns |     28.7788 ns |  1,916.77 ns |

#### Accessing a property;
 |           Method |           Mean |         Error |        StdDev |
 |----------------- |---------------:|--------------:|--------------:|
 | CompileEveryTime | 78,608.9694 ns | 1,552.6065 ns | 2,370.9942 ns |
 |      CompileOnce |      0.8400 ns |     0.0444 ns |     0.0415 ns |
 |         Evaluate |    915.2006 ns |    13.9449 ns |    13.0441 ns |


So Evaluate is 1-2 order of magnitudes faster than compiling every time, although much  much slower than
caching the compiled function. If you can predict that the expressions you will use will be few,
compiling and caching is still a better option. Otherwise, evaluate away!