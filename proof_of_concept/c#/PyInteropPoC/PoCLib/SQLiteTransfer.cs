using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PoCLib
{
    public class SQLiteTransfer : PoCClass
    {
        private SQLiteClient client;

        [GlobalSetup]
        public void Setup()
        {
            client = new SQLiteClient();
            client.CreateDataBase();
        }

        [Benchmark]
        public override void WriteIntData()
        {
            string sql = $"insert into param_int (pid, pos, value) values (1, 0, {intA}), (1, 1, {intB});";
            client.ExecuteCommand(sql);
        }

        [Benchmark]
        public override int[] ReadIntData()
        {
            return null;
        }

        [Benchmark]
        public override void WriteIntArrayData()
        {
            string sqlArray = $"insert into param_int_array (pid, pos) values (1, 0), (1, 1)";
            client.ExecuteCommand(sqlArray);
            string sqlArrayData = "insert into param_int_array_value (pid, pos, array_index, value) values \n";
            sqlArrayData += GetSqlInsertValuesForArray(1, 0, intArrayA);
            sqlArrayData += ", ";
            sqlArrayData += GetSqlInsertValuesForArray(1, 1, intArrayB);
            client.ExecuteCommand(sqlArrayData);
        }

        private string GetSqlInsertValuesForArray<T>(int pid, int arrayPos, T[] array) where T : IFormattable
        {
            int i = 0;
            return string.Join(",", array.Select(s => $"({pid}, {arrayPos}, {i++}, {s.ToString(null, CultureInfo.InvariantCulture)})"));
        }

        [Benchmark]
        public override int[][] ReadIntArrayData()
        {
            return null;
        }

        [Benchmark]
        public override void WriteFloatData()
        {
            string sql = 
                $"insert into param_float (pid, pos, value) values " +
                $"(1, 0, {floatA.ToString(null, CultureInfo.InvariantCulture)}), " +
                $"(1, 1, {floatB.ToString(null, CultureInfo.InvariantCulture)})";
            client.ExecuteCommand(sql);
        }

        [Benchmark]
        public override double[] ReadFloatData()
        {
            return null;
        }

        [Benchmark]
        public override void WriteFloatArrayData()
        {
            string sqlArray = $"insert into param_float_array (pid, pos) values (1, 0), (1, 1)";
            client.ExecuteCommand(sqlArray);
            string sqlArrayData = "insert into param_float_array_value (pid, pos, array_index, value) values \n";
            sqlArrayData += GetSqlInsertValuesForArray(1, 0, floatArrayA);
            sqlArrayData += ", ";
            sqlArrayData += GetSqlInsertValuesForArray(1, 1, floatArrayB);
            client.ExecuteCommand(sqlArrayData);
        }

        [Benchmark]
        public override double[][] ReadFloatArrayData()
        {
            return null;
        }
    }
}
