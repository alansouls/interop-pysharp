using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PoCLib
{
    public abstract class PoCClass
    {
        protected int intA = 1;
        protected int intB = 2;
        protected int[] intArrayA = GetRandomIntArray(1000000, 0, 1000000);
        protected int[] intArrayB = GetRandomIntArray(1000000, 0, 1000000);
        protected double floatA = 1.123;
        protected double floatB = 2.431;
        protected double[] floatArrayA = GetRandomFloatArray(1000000, 0, 1000000);
        protected double[] floatArrayB = GetRandomFloatArray(1000000, 0, 1000000);

        private static int[] GetRandomIntArray(int size, int min, int max)
        {
            Random random = new Random();
            var result = new int[size];
            for (var i = 0; i < size; ++i)
            {
                result[i] = random.Next(min, max);
            }
            return result;
        }

        private static double[] GetRandomFloatArray(int size, int min, int max)
        {
            Random random = new Random();
            var result = new double[size];
            for (var i = 0; i < size; ++i)
            {
                var randomValue = random.NextDouble() * (max - min) + min; ;
                result[i] = randomValue;
            }
            return result;
        }

        public void Run()
        {
            WriteIntData();
            ReadIntData();
            WriteIntArrayData();
            ReadIntArrayData();
            WriteFloatData();
            ReadFloatData();
            WriteFloatArrayData();
            ReadFloatArrayData();
        }

        public abstract void WriteIntData();
        public abstract int[] ReadIntData();
        public abstract void WriteIntArrayData();
        public abstract int[][] ReadIntArrayData();
        public abstract void WriteFloatData();
        public abstract double[] ReadFloatData();
        public abstract void WriteFloatArrayData();
        public abstract double[][] ReadFloatArrayData();

        protected string ArrayToString<T>(T[] array) where T : IFormattable
        {
            if (typeof(T) == typeof(double))
            {
                return string.Join(",", array.Select(s => s.ToString(null, CultureInfo.InvariantCulture)));
            }
            else
                return string.Join(",", array);
        }
    }
}
