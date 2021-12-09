﻿using BaseInterop.Infrastructure;
using PythonInterop.Infrastructure;
using System.Threading.Tasks;

namespace SampleInterop.Services
{
    public class FacialRecognitionService : PythonService, IFacialRecognitionService
    {
        public FacialRecognitionService(string path, ICodeGenerator codeGenerator, IDataTransferer dataTransferer) 
            : base(path, "facial_rec", codeGenerator, dataTransferer)
        {
        }

        public async Task<bool> CheckFacesAsync(string knownImagePath, string unknownImagePath)
        {
            var processId = await StartServiceAsync("check_facial_validation", knownImagePath, unknownImagePath);
            return await RetrieveReturnAsync<bool>(processId);
        }
    }
}