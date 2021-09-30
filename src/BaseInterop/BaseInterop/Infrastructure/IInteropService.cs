using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseInterop.Infrastructure
{
    public interface IInteropService
    {
        int Id { get; }
        string Name { get; }
        Task StartServiceAsync();

        Task<object> RetrieveReturnAsync();
    }
}
