using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.Infrastructure
{
    public interface ICodeGenerator
    {
        string GenerateCommunicatorCode(string dataPath, string resultDataPath, string filePath, string functionName, int paramCount);
    }
}
