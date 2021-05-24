using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PoCLib
{
    public class TextFileTransfer : PoCClass
    {

        [Benchmark]
        public override void WriteIntData() 
        {
            intA = 1;
            intB = 2;
            string contents = intA.ToString() + "\n" + intB.ToString();
            File.WriteAllText("int_file.txt", contents);
        }


        [Benchmark]
        public override void WriteIntArrayData()
        {
            string contents = ArrayToString(intArrayA) + "\n" + ArrayToString(intArrayB);
            File.WriteAllText("int_array_file.txt", contents);
        }


        [Benchmark]
        public override void WriteFloatData()
        {
            string contents = floatA.ToString() + "\n" + floatB.ToString();
            File.WriteAllText("float_file.txt", contents);
        }


        [Benchmark]
        public override void WriteFloatArrayData()
        {
            string contents = ArrayToString(floatArrayA) + "\n" + ArrayToString(floatArrayB);
            File.WriteAllText("float_array_file.txt", contents);
        }


        [Benchmark]
        public override int[] ReadIntData()
        {
            var content = File.ReadAllText("int_file.txt");
            var contentByLines = content.Split('\n');
            int[] result = new int[contentByLines.Count()];
            for (var i = 0; i < contentByLines.Count(); ++i)
            {
                result[i] = int.Parse(contentByLines[i]);
            }

            return result;
        }


        [Benchmark]
        public override int[][] ReadIntArrayData()
        {
            var content = File.ReadAllText("int_array_file.txt");
            var contentByLines = content.Split('\n');
            int[][] result = new int[contentByLines.Count()][];
            for (var i = 0; i < contentByLines.Count(); ++i)
            {
                var line = contentByLines[i];
                var contentByComma = line.Split(',');
                int[] arrayResult = new int[contentByComma.Count()];
                for (var j = 0; j < contentByComma.Count(); ++j)
                {
                    arrayResult[j] = int.Parse(contentByComma[j]);
                }
                result[i] = arrayResult;
            }

            return result;
        }


        [Benchmark]
        public override double[] ReadFloatData()
        {
            var content = File.ReadAllText("float_file.txt");
            var contentByLines = content.Split('\n');
            double[] result = new double[contentByLines.Count()];
            for (var i = 0; i < contentByLines.Count(); ++i)
            {
                result[i] = double.Parse(contentByLines[i]);
            }

            return result;
        }


        [Benchmark]
        public override double[][] ReadFloatArrayData()
        {
            var content = File.ReadAllText("float_array_file.txt");
            var contentByLines = content.Split('\n');
            double[][] result = new double[contentByLines.Count()][];
            for (var i = 0; i < contentByLines.Count(); ++i)
            {
                var line = contentByLines[i];
                var contentByComma = line.Split(',');
                double[] arrayResult = new double[contentByComma.Count()];
                for (var j = 0; j < contentByComma.Count(); ++j)
                {
                    arrayResult[j] = double.Parse(contentByComma[j], CultureInfo.InvariantCulture);
                }
                result[i] = arrayResult;
            }

            return result;
        }
    }
}
