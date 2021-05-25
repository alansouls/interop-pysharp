``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.201
  [Host]     : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT


```
|              Method |        Mean |     Error |      StdDev |      Median |
|-------------------- |------------:|----------:|------------:|------------:|
|        WriteIntData |    380.3 μs |  19.98 μs |    58.92 μs |    357.1 μs |
|         ReadIntData |    382.8 μs |   9.05 μs |    26.39 μs |    380.6 μs |
|   WriteIntArrayData | 39,404.0 μs | 777.93 μs | 1,164.36 μs | 39,440.8 μs |
|    ReadIntArrayData | 13,687.6 μs | 264.39 μs |   411.63 μs | 13,753.6 μs |
|      WriteFloatData |    384.1 μs |  19.68 μs |    57.71 μs |    362.8 μs |
|       ReadFloatData |    363.1 μs |   6.43 μs |     6.61 μs |    365.1 μs |
| WriteFloatArrayData | 52,110.9 μs | 704.36 μs |   588.18 μs | 51,791.6 μs |
|  ReadFloatArrayData | 20,131.2 μs | 398.96 μs |   729.53 μs | 20,333.8 μs |
