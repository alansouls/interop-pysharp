using FluentAssertions;
using PythonInterop.Infrastructure;
using System;
using Xunit;

namespace PythonInterop.Tests
{
    public class PythonCodeGeneratorTests
    {
        private readonly PythonCodeGenerator _codeGenerator;
        public PythonCodeGeneratorTests()
        {
            _codeGenerator = new PythonCodeGenerator();
        }

        [Fact]
        public void ShouldGenerateCommunicatorCodeCorrectly()
        {
            var code = _codeGenerator.GenerateCommunicatorCode("in.dat", "out.dat", "testmodule", "testfunction", 3);

            var comparison = @"import sys
import testmodule
from byte_encode_decode import VarType, ByteEncodeDecode, TypeSize
processId = int(sys.argv[1])
with open('in.dat'.format(processId), 'br') as f:
    data = ByteEncodeDecode.read_from_bytes(f.read())
    f.close()
param0 = data[0]
param1 = data[1]
param2 = data[2]
result = testmodule.testfunction(param0, param1, param2)
with open('out.dat'.format(processId), 'bw') as f:
    buffer = ByteEncodeDecode.to_byte(result)
    f.write(buffer)
    f.close()";

            code.Replace("\r", "").Should().Be(comparison.Replace("\r", "")); //ignore carriage return comparing the 2 codes
        }
    }
}
