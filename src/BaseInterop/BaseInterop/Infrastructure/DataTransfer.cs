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
        private readonly List<ICodecAdapter> _adapters;

        public DataTransfer()
        {
            _adapters = new List<ICodecAdapter>();
        }

        public DataTransfer AddDefaultAdapter()
        {
            return AddAdapter(new DefaultCodecAdapter());
        }

        public DataTransfer AddAdapter(ICodecAdapter adapter)
        {
            if (!_adapters.Any(s => s.GetType() == adapter.GetType()))
                _adapters.Add(adapter);

            return this;
        }

        public List<Type> GetSupportedTypes()
        {
            return _adapters.SelectMany(s => s.GetSupportedTypes()).ToList();
        }

        public T ReadData<T>(DataReadOptions options)
        {
            if (!IsTypeSupported<T>())
                throw new ArgumentException("This type is not supported by this data transfer class.");
            var filePath = options.Path;
            using var fileStream = File.OpenRead(filePath);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer);
            if (!IsTypeSupported(typeof(T)))
                throw new ArgumentException("This type is not supported by this data transfer class.");
            var adapter = _adapters.First(s => s.GetSupportedTypes().Contains(typeof(T)));
            return adapter.ReadDataFromBytes<T>(buffer);
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
            using var fileStream = File.OpenWrite(filePath);
            fileStream.Position = 0;
            var buffers = new byte[data.Length][];
            WriteDataToBuffers(data, buffers);
            foreach (var buffer in buffers)
            {
                fileStream.Write(buffer);
            }
        }

        private void WriteDataToBuffers(object[] data, byte[][] buffers)
        {
            int i = 0;
            foreach (var dataItem in data)
            {
                var type = dataItem.GetType();
                if (!IsTypeSupported(type))
                    throw new ArgumentException("This type is not supported by this data transfer class.");
                var adapter = _adapters.First(s => s.GetSupportedTypes().Contains(type));
                buffers[i] = adapter.GetBytes(dataItem);
                ++i;
            }
        }
    }
}
