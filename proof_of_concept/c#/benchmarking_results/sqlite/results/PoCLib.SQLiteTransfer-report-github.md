``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.201
  [Host]     : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT


```
|              Method |                   Mean |               Error |                StdDev |                 Median |
|-------------------- |-----------------------:|--------------------:|----------------------:|-----------------------:|
|        WriteIntData |    105,529,019.5402 ns |   4,824,267.9427 ns |    13,206,355.6419 ns |    102,939,200.0000 ns |
|         ReadIntData |              0.0026 ns |           0.0063 ns |             0.0059 ns |              0.0000 ns |
|   WriteIntArrayData |  9,534,789,085.7143 ns | 133,087,616.5954 ns |   117,978,705.5839 ns |  9,513,699,450.0000 ns |
|    ReadIntArrayData |              0.0944 ns |           0.0632 ns |             0.0843 ns |              0.0730 ns |
|      WriteFloatData |    113,779,788.2828 ns |   4,527,065.2582 ns |    13,277,098.7376 ns |    110,641,800.0000 ns |
|       ReadFloatData |              0.0655 ns |           0.0606 ns |             0.0673 ns |              0.0415 ns |
| WriteFloatArrayData | 16,243,732,067.0000 ns | 847,550,764.1206 ns | 2,499,022,550.7205 ns | 16,856,396,100.0000 ns |
|  ReadFloatArrayData |              0.0210 ns |           0.0241 ns |             0.0225 ns |              0.0133 ns |
