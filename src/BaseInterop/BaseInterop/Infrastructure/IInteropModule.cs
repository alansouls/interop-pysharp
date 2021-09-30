using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.Infrastructure
{
    public interface IInteropModule
    {
        void RunServiceNoReturn(string serviceName);

        T RunService<T>(string serviceName);
    }
}
