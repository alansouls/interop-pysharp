using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.DataTransfer
{
    public class DefaultCodecAdapter : ICodecAdapter
    {
        public byte[] GetBytes(object data)
        {
            var type = data.GetType();
            if (type == typeof(int))
                return ByteEncodeDecode.GetBytes((int)data);
            else if (type == typeof(float) || type == typeof(double))
                return ByteEncodeDecode.GetBytes((double)data);
            else if (type == typeof(string))
                return ByteEncodeDecode.GetBytes((string)data);
            else if (type == typeof(bool))
                return ByteEncodeDecode.GetBytes((bool)data);
            else if (type.GetInterfaces().Any(s => s == typeof(IEnumerable<int>)))
            {
                var convertedData = (IEnumerable<int>)data;
                return ByteEncodeDecode.GetBytes(convertedData.ToArray());
            }
            else if (type.GetInterfaces().Any(s => s == typeof(IEnumerable<double>)))
            {
                var convertedData = (IEnumerable<double>)data;
                return ByteEncodeDecode.GetBytes(convertedData.ToArray());
            }

            throw new ArgumentException("Data type not supported!");
        }

        public List<Type> GetSupportedTypes()
        {
            return new List<Type>
            {
                typeof(int),
                typeof(double),
                typeof(string),
                typeof(bool)
            };
        }

        public T ReadDataFromBytes<T>(byte[] buffer)
        {
            var resultList = ByteEncodeDecode.ReadFromBytes(buffer, out var typesOut);
            if (typesOut[0] != typeof(T))
                throw new Exception("The type read from buffer was different from the type requested.");
            var result = resultList[0];
            return (T)result;
        }
    }
}
