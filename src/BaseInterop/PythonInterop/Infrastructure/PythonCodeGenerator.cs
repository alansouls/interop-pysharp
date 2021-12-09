using BaseInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonInterop.Infrastructure
{
    public class PythonCodeGenerator : ICodeGenerator
    {
        public string GenerateCommunicatorCode(string dataPath, string resultDataPath, string moduleName, string functionName, int paramCount)
        {
            StringBuilder sb = new();
            var importsPart =
                $@"
import {moduleName}
from byte_encode_decode import VarType, ByteEncodeDecode, TypeSize";
            sb.Append(importsPart);
            var dataReadPart = $@"
with open('{dataPath}', 'br') as f:
    data = ByteEncodeDecode.read_from_bytes(f.read())
    f.close()";
            sb.Append(dataReadPart);
            string[] paramsMountPart = new string[paramCount];
            string[] parameterStringList = new string[paramCount];
            for (int i = 0; i < paramCount; ++i)
            {
                paramsMountPart[i] = $"param{i} = data[{i}]";
                sb.Append(paramsMountPart[i]);
                parameterStringList[i] = $"param{i}";
            }
            var executeFunctionPart = $"result = {functionName}({string.Join(", ", parameterStringList)})";
            sb.Append(executeFunctionPart);
            var resultWritePart = $@"
with open('{resultDataPath}', 'bw') as f:
    buffer = ByteEncodeDecode.to_byte(result)
    f.write(buffer)
    f.close()";
            sb.Append(resultWritePart);
            return sb.ToString();
        }
    }
}
