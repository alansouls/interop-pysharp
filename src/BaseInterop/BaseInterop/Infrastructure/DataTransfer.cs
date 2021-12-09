using BaseInterop.DataTransfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.Infrastructure
{
    public class DataTransfer : IDataTransferer
    {
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

        public T ReadData<T>(DataReadOptions options)
        {
            if (!IsTypeSupported<T>())
                throw new ArgumentException("This type is not supported by this data transfer class.");
            var filePath = options.Path;
            var fileStream = File.OpenRead(filePath);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer);
            var resultList = ByteEncodeDecode.ReadFromBytes(buffer, out var typesOut);
            if (typesOut[0] != typeof(T))
                throw new Exception("The type read from the file was different from the type requested.");
            var result = resultList[0];
            return (T)result;
        }

        private bool IsTypeSupported<T>()
        {
            return IsTypeSupported(typeof(T));
        }

        private bool IsTypeSupported(Type t)
        {
            return GetSupportedTypes().Contains(t);
        }

        public void TransferData(DataTransferOptions options, params object[] data)
        {
            var filePath = options.Path;
            var fileStream = File.OpenWrite(filePath);
            var buffers = new byte[data.Length][];
            int i = 0;
            foreach (var dataItem in data)
            {
                var type = dataItem.GetType();
                if (!IsTypeSupported(type))
                    throw new ArgumentException("This type is not supported by this data transfer class.");
                if (type == typeof(int))
                buffers[i] = ByteEncodeDecode.GetBytes(dataItem);
                ++i;
            }
        }
    }
}
