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
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDataTransferer _dataTransferer;

        private readonly Dictionary<string, string> _functionConnectorMap;

        private readonly Dictionary<int, Process> _processMap;
        private int processCount;

        public PythonService(string path, ICodeGenerator codeGenerator, IDataTransferer dataTransferer)
        {
            _path = path;
            _codeGenerator = codeGenerator;
            _dataTransferer = dataTransferer;

            _functionConnectorMap = new Dictionary<string, string>();
            _processMap = new Dictionary<int, Process>();
            processCount = 0;
        }

        protected override async Task<int> StartServiceAsync(string functionName, params object[] parameters)
        {
            if (!_functionConnectorMap.TryGetValue(functionName, out string connectorPath))
            {
                var connector = _codeGenerator.GenerateCommunicatorCode(_path, functionName);
                connectorPath = Path.Combine(Directory.GetCurrentDirectory(), $"{functionName}_connector.py");
                var file = File.Create(connectorPath);
                var buffer = Encoding.UTF8.GetBytes(connector);
                await file.WriteAsync(buffer.AsMemory(0, buffer.Length));
                _functionConnectorMap.Add(functionName, connectorPath);
            }

            var processId = ++processCount;
            PreparePythonTransfer(processId, parameters);

            var startInfo = GetPythonProcessStartInfo(connectorPath, processId);
            var process = Process.Start(startInfo);
            _processMap.Add(processId, process);

            return processId;
        }

        private void PreparePythonTransfer(int processId, object[] parameters)
        {
            var options = new DataTransferOptions()
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), $"_in_{processId}.dat")
            };
            _dataTransferer.TransferData(options, parameters);
        }

        private static ProcessStartInfo GetPythonProcessStartInfo(string connectorPath, int processId)
        {
            return new ProcessStartInfo
            {
                FileName = "python.exe",
                Arguments = string.Format("{0}, {1}", connectorPath, processId),
                UseShellExecute = false
            };
        }

        protected override async Task<T> RetrieveReturnAsync<T>(int processId)
        {
            var process = _processMap[processId];
            await process.WaitForExitAsync();
            var options = new DataReadOptions
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), $"_out_{processId}.dat")
            };
            var data = _dataTransferer.ReadData<T>(options);
            CleanProcessFiles(processId);
            return data;
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
