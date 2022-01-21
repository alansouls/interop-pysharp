using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.DataTransfer
{
    public interface ICodecAdapter
    {
        List<Type> GetSupportedTypes();

        byte[] GetBytes(object data);

        T ReadDataFromBytes<T>(byte[] buffer);
    }
}
