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
            else if (type == typeof(float))
                return ByteEncodeDecode.GetBytes((float)data);
            else if (type == typeof(string))
                return ByteEncodeDecode.GetBytes((string)data);
            else if (type == typeof(bool))
                return ByteEncodeDecode.GetBytes((bool)data);

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
    }
}
