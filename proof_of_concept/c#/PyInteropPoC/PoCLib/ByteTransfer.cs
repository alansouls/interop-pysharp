using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PoCLib
{
    public class ByteTransfer : PoCClass
    {

        [Benchmark]
        public override void WriteIntData()
        {
            var bytesA = ByteEncodeDecode.GetBytes(intA);
            var bytesB = ByteEncodeDecode.GetBytes(intB);
            var result = new byte[bytesA.Length + bytesB.Length];
            bytesA.CopyTo(result, 0);
            bytesB.CopyTo(result, bytesA.Length);
            File.WriteAllBytes("bytes_int.data", result);
        }

        [Benchmark]
        public override int[] ReadIntData()
        {
            var result = new int[2];
            var buffer = File.ReadAllBytes("bytes_int.data");
            var resultObjects = ByteEncodeDecode.ReadFromBytes(buffer, out List<Type> _);
            result[0] = Convert.ToInt32(resultObjects[0]);
            result[1] = Convert.ToInt32(resultObjects[1]);

            return result;
        }

        [Benchmark]
        public override void WriteIntArrayData()
        {
            var bytesA = ByteEncodeDecode.GetBytes(intArrayA);
            var bytesB = ByteEncodeDecode.GetBytes(intArrayB);
            var result = new byte[bytesA.Length + bytesB.Length];
            bytesA.CopyTo(result, 0);
            bytesB.CopyTo(result, bytesA.Length);
            File.WriteAllBytes("bytes_int_array.data", result);
        }

        [Benchmark]
        public override int[][] ReadIntArrayData()
        {
            var result = new int[2][];
            var buffer = File.ReadAllBytes("bytes_int_array.data");
            var resultObjects = ByteEncodeDecode.ReadFromBytes(buffer, out List<Type> _);
            result[0] = resultObjects[0] as int[];
            result[1] = resultObjects[1] as int[];

            return result;
        }

        [Benchmark]
        public override void WriteFloatData()
        {
            var bytesA = ByteEncodeDecode.GetBytes(floatA);
            var bytesB = ByteEncodeDecode.GetBytes(floatB);
            var result = new byte[bytesA.Length + bytesB.Length];
            bytesA.CopyTo(result, 0);
            bytesB.CopyTo(result, bytesA.Length);
            File.WriteAllBytes("bytes_float.data", result);
        }

        [Benchmark]
        public override double[] ReadFloatData()
        {
            var result = new double[2];
            var buffer = File.ReadAllBytes("bytes_float.data");
            var resultObjects = ByteEncodeDecode.ReadFromBytes(buffer, out List<Type> _);
            result[0] = Convert.ToDouble(resultObjects[0]);
            result[1] = Convert.ToDouble(resultObjects[1]);

            return result;
        }

        [Benchmark]
        public override void WriteFloatArrayData()
        {
            var bytesA = ByteEncodeDecode.GetBytes(floatArrayA);
            var bytesB = ByteEncodeDecode.GetBytes(floatArrayB);
            var result = new byte[bytesA.Length + bytesB.Length];
            bytesA.CopyTo(result, 0);
            bytesB.CopyTo(result, bytesA.Length);
            File.WriteAllBytes("bytes_float_array.data", result);
        }

        [Benchmark]
        public override double[][] ReadFloatArrayData()
        {
            var result = new double[2][];
            var buffer = File.ReadAllBytes("bytes_float_array.data");
            var resultObjects = ByteEncodeDecode.ReadFromBytes(buffer, out List<Type> _);
            result[0] = resultObjects[0] as double[];
            result[1] = resultObjects[1] as double[];

            return result;
        }
    }
}
