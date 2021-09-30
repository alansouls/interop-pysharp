using BaseInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonInterop.Infrastructure
{
    public class PythonService : IInteropService
    {
        public int Id { get; }

        public string Name { get; }

        public string Path { get; set; }

        public PythonService(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Task StartServiceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<object> RetrieveReturnAsync()
        {
            throw new NotImplementedException();
        }
    }
}
