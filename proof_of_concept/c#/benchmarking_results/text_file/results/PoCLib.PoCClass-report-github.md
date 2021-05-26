``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.201
  [Host]     : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT


```
|              Method |           Mean |        Error |       StdDev |         Median |
|-------------------- |---------------:|-------------:|-------------:|---------------:|
|        WriteIntData |       357.4 μs |     18.15 μs |     53.52 μs |       332.6 μs |
|   WriteIntArrayData |   138,821.4 μs |  2,726.04 μs |  4,162.95 μs |   138,042.0 μs |
|      WriteFloatData |       376.6 μs |     18.37 μs |     54.17 μs |       350.5 μs |
| WriteFloatArrayData |   751,595.3 μs | 14,959.45 μs | 30,558.20 μs |   737,750.9 μs |
|         ReadIntData |       110.7 μs |      1.60 μs |      1.49 μs |       111.3 μs |
|    ReadIntArrayData |   284,359.6 μs |  5,596.83 μs |  8,026.81 μs |   284,162.0 μs |
|       ReadFloatData |       120.7 μs |      2.37 μs |      2.63 μs |       120.6 μs |
|  ReadFloatArrayData | 1,080,299.9 μs |  9,779.04 μs |  8,668.86 μs | 1,080,766.6 μs |
