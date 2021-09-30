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
        Type GetSupportedTypes();

        void TransferData<T>(T data, DataTransferOptions options);

        void ReadData<T>(DataTransferOptions options);
    }
}
