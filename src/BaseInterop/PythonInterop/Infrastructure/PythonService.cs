using BaseInterop.DataTransfer;
using BaseInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonInterop.Infrastructure
{
    public class PythonService : InteropService, IInteropService
    {

        private readonly string _path;
        private readonly string _moduleName;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDataTransferer _dataTransferer;

        private readonly Dictionary<string, string> _functionConnectorMap;

        private readonly Dictionary<int, Process> _processMap;
        private int processCount;

        public PythonService(string path, string moduleName, ICodeGenerator codeGenerator, IDataTransferer dataTransferer)
        {
            _path = path;
            _moduleName = moduleName;
            _codeGenerator = codeGenerator;
            _dataTransferer = dataTransferer;

            _functionConnectorMap = new Dictionary<string, string>();
            _processMap = new Dictionary<int, Process>();
            processCount = 0;
        }

        protected override async Task<int> StartServiceAsync(string functionName, params object[] parameters)
        {
            string connectorPath = GetConnectorPath(functionName, parameters.Length);
            var processId = ++processCount;
            PreparePythonTransfer(processId, parameters);

            var startInfo = GetPythonProcessStartInfo(connectorPath, processId);
            var process = Process.Start(startInfo);
            _processMap.Add(processId, process);

            return processId;
        }

        private string GetConnectorPath(string functionName, int paramsCount)
        {
            if (!_functionConnectorMap.TryGetValue(functionName, out string connectorPath))
            {
                var connector = _codeGenerator.GenerateCommunicatorCode(Path.Combine(Directory.GetCurrentDirectory(), "_in_{0}.dat"), Path.Combine(Directory.GetCurrentDirectory(), "_out_{0}.dat"),
                    _moduleName, functionName, paramsCount);
                connectorPath = Path.Combine(Directory.GetCurrentDirectory(), $"{functionName}_connector.py");
                using var fileStream = File.OpenWrite(connectorPath);
                fileStream.Position = 0;
                var buffer = Encoding.UTF8.GetBytes(connector);
                fileStream.Write(buffer);
                _functionConnectorMap.Add(functionName, connectorPath);
            }

            return connectorPath;
        }

        private void PreparePythonTransfer(int processId, object[] parameters)
        {
            var options = new DataTransferOptions()
            {
                Path = GetDataInPath(processId)
            };
            _dataTransferer.TransferData(options, parameters);
        }

        private static string GetDataInPath(int processId)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), $"_in_{processId}.dat");
        }

        private static ProcessStartInfo GetPythonProcessStartInfo(string connectorPath, int processId)
        {
            return new ProcessStartInfo
            {
                FileName = "python.exe",
                Arguments = string.Format("{0} {1}", connectorPath, processId),
                RedirectStandardError = true,
                UseShellExecute = false
            };
        }

        protected override async Task<T> RetrieveReturnAsync<T>(int processId)
        {
            var process = _processMap[processId];
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
            {
                throw new Exception($"Error executing python script {process.StandardError.ReadToEnd()}");
            }
            var options = new DataReadOptions
            {
                Path = GetDataOutPath(processId)
            };
            var data = _dataTransferer.ReadData<T>(options);
            CleanProcessFiles(processId);
            return data;
        }

        private static string GetDataOutPath(int processId)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), $"_out_{processId}.dat");
        }

        private static void CleanProcessFiles(int processId)
        {
            File.Delete(Directory.GetCurrentDirectory() + $"_out_{processId}.dat");
            File.Delete(Directory.GetCurrentDirectory() + $"_in_{processId}.dat");
        }

        public override void Dispose()
        {
            foreach (var file in _functionConnectorMap.Values)
                File.Delete(file);

            GC.SuppressFinalize(this);
        }
    }
}
