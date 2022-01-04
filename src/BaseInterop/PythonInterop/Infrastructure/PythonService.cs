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
        private readonly Guid _uniqueIdentifier;
        private readonly string _folderPath;
        private readonly string _moduleName;
        private readonly string _workingDirectory;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDataTransferer _dataTransferer;

        private readonly Dictionary<string, string> _functionConnectorMap;

        private readonly Dictionary<int, Process> _processMap;
        private int processCount;

        public PythonService(string folderPath, string moduleName, ICodeGenerator codeGenerator, IDataTransferer dataTransferer)
        {
            _uniqueIdentifier = Guid.NewGuid();
            _folderPath = folderPath;
            _workingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "sharpinterop", $"python_{_uniqueIdentifier}");
            Directory.CreateDirectory(_workingDirectory);
            CopyPythonFilesToWorkingDirectory();
            _moduleName = moduleName;
            _codeGenerator = codeGenerator;
            _dataTransferer = dataTransferer;

            _functionConnectorMap = new Dictionary<string, string>();
            _processMap = new Dictionary<int, Process>();
            processCount = 0;
        }

        private void CopyPythonFilesToWorkingDirectory()
        {
            foreach (var file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "py_scripts")))
            {
                File.Copy(file, Path.Combine(_workingDirectory, Path.GetFileName(file)));
            }

            foreach (var file in Directory.GetFiles(_folderPath))
            {
                File.Copy(file, Path.Combine(_workingDirectory, Path.GetFileName(file)));
            }
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
                var connector = _codeGenerator.GenerateCommunicatorCode(Path.Combine(_workingDirectory, "_in_{0}.dat"), Path.Combine(_workingDirectory, "_out_{0}.dat"),
                    _moduleName, functionName, paramsCount);
                connectorPath = Path.Combine(_workingDirectory, $"{functionName}_connector.py");
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

        private string GetDataInPath(int processId)
        {
            return Path.Combine(_workingDirectory, $"_in_{processId}.dat");
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

        private string GetDataOutPath(int processId)
        {
            return Path.Combine(_workingDirectory, $"_out_{processId}.dat");
        }

        private void CleanProcessFiles(int processId)
        {
            File.Delete(Path.Combine(_workingDirectory, $"_out_{processId}.dat"));
            File.Delete(Path.Combine(_workingDirectory, $"_in_{processId}.dat"));
        }

        public override void Dispose()
        {
            Directory.Delete(_workingDirectory, true);
            GC.SuppressFinalize(this);
        }
    }
}
