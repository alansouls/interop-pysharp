using BaseInterop.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.Infrastructure
{
    public interface IDataTransferer
    {
        List<Type> GetSupportedTypes();

        void TransferData(DataTransferOptions options, params object[] data);

        T ReadData<T>(DataReadOptions options);
    }
}
