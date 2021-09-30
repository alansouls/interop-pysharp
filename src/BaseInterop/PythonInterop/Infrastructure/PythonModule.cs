using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseInterop.Infrastructure;

namespace PythonInterop.Infrastructure
{
    public class PythonModule : IInteropModule
    {
        int serviceCount = 0;
        private readonly Hashtable servicePool;

        public PythonModule()
        {
            servicePool = new Hashtable();
        }

        public T RunService<T>(string serviceName)
        {
            servicePool.Add(serviceCount++, new IInteropService());
        }

        public void RunServiceNoReturn(string serviceName)
        {
            throw new NotImplementedException();
        }
    }
}
