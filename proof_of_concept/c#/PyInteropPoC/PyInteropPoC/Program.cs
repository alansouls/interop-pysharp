using BenchmarkDotNet.Running;
using PoCLib;
using System;

namespace PyInteropPoC
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ByteTransfer>();
        }
    }
}
