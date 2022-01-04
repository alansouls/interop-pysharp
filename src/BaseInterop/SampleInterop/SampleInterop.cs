using SampleInterop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleInterop
{
    public class SampleInterop : IDisposable
    {
        private readonly FacialRecognitionService _facialReconService;
        public SampleInterop(FacialRecognitionService facialReconService)
        {
            _facialReconService = facialReconService;
        }
        public void Run(string originalPath, string comparingPath)
        {
            Console.WriteLine(_facialReconService.CheckFacesAsync(originalPath, comparingPath).Result ? "Verificação sucedida" : "Verificação falhou");
        }

        public void Dispose()
        {
            _facialReconService.Dispose();
        }
    }
}
