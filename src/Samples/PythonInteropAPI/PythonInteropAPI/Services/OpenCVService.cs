using BaseInterop.Infrastructure;
using PythonInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PythonInteropAPI.Services
{
    public class OpenCVService : PythonService, IOpenCVService
    {
        public OpenCVService(ICodeGenerator codeGenerator, IDataTransferer dataTransferer) 
            : base("python_codes", "interop_opencv", codeGenerator, dataTransferer)
        {
        }

        public async Task<string> BlurAsync(string image, int intensity)
        {
            var projectId = await StartServiceAsync("blur", image, intensity);
            return await RetrieveReturnAsync<string>(projectId);
        }

        public async Task<string> ContourAsync(string image)
        {
            var projectId = await StartServiceAsync("contour", image);
            return await RetrieveReturnAsync<string>(projectId);
        }

        public async Task<string> SegmentationAsync(string image)
        {
            var projectId = await StartServiceAsync("segmentation", image);
            return await RetrieveReturnAsync<string>(projectId);
        }
    }
}
