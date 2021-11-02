using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.Infrastructure
{
    public abstract class InteropService : IInteropService, IDisposable
    {
        protected abstract Task<int> StartServiceAsync(string functionName, params object[] parameters);

        protected abstract Task<T> RetrieveReturnAsync<T>(int processId);

        public abstract void Dispose();
    }
}
