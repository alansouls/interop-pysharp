using BenchmarkDotNet.Running;
using PoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PyInteropPoC
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            var pyProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "py.exe",
                    Arguments = $"{dir}\\connector_file.py",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            int a = int.Parse(Console.ReadLine());
            int b = int.Parse(Console.ReadLine());

            var bytesA = ByteEncodeDecode.GetBytes(a);
            var bytesB = ByteEncodeDecode.GetBytes(b);
            var resultBytes = new byte[bytesA.Length + bytesB.Length];
            bytesA.CopyTo(resultBytes, 0);
            bytesB.CopyTo(resultBytes, bytesA.Length);
            File.WriteAllBytes("input.data", resultBytes);

            pyProcess.Start();
            pyProcess.WaitForExit();
            if (pyProcess.ExitCode != 0)
            {
                Console.Write(pyProcess.StandardError.ReadToEnd());
                throw new Exception("Python process failed.");
            }

            var result = new int[2];
            var buffer = File.ReadAllBytes("output.data");
            var resultObjects = ByteEncodeDecode.ReadFromBytes(buffer, out List<Type> _);
            result[0] = Convert.ToInt32(resultObjects[0]);

            Console.WriteLine(result[0]);
        }
    }
}
