using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PythonInteropAPI.Extensions;
using PythonInteropAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PythonInteropAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenCVController : ControllerBase
    {
        private readonly IOpenCVService _service;
        public  OpenCVController(IOpenCVService service)
        {
            _service = service;
        }

        [HttpPost("blur")]
        public async Task<IActionResult> Blur(IFormFile file, [FromQuery]int intensity = 10)
        {
            string sourceImagePath = file.GetLocalFilePath();

            var output = await _service.BlurAsync(sourceImagePath, intensity);

            return CleanUpFile(sourceImagePath, output);
        }

        [HttpPost("contour")]
        public async Task<IActionResult> Contour(IFormFile file)
        {
            string sourceImagePath = file.GetLocalFilePath();

            var output = await _service.ContourAsync(sourceImagePath);

            return CleanUpFile(sourceImagePath, output);
        }

        [HttpPost("segmentation")]
        public async Task<IActionResult> Segmentation(IFormFile file)
        {
            string sourceImagePath = file.GetLocalFilePath();

            var output = await _service.SegmentationAsync(sourceImagePath);
            return CleanUpFile(sourceImagePath, output);
        }

        private IActionResult CleanUpFile(string sourceImagePath, string output)
        {
            System.IO.File.Delete(sourceImagePath);

            var fileBytes = System.IO.File.ReadAllBytes(output);

            var result = File(fileBytes, "image/png");

            System.IO.File.Delete(output);

            return result;
        }
    }
}
