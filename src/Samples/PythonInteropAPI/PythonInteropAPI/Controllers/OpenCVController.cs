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
        private readonly ILogger<OpenCVController> _logger;
        public  OpenCVController(IOpenCVService service, ILogger<OpenCVController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("blur")]
        public async Task<IActionResult> Blur(IFormFile file, [FromQuery]int intensity = 10)
        {
            var start = DateTime.Now;
            string sourceImagePath = file.GetLocalFilePath();

            var output = await _service.BlurAsync(sourceImagePath, intensity);

            var result = CleanUpFile(sourceImagePath, output);
            _logger.LogInformation("Took {} ms to execute", (DateTime.Now - start).TotalMilliseconds);
            return result;
        }

        [HttpPost("contour")]
        public async Task<IActionResult> Contour(IFormFile file)
        {
            var start = DateTime.Now;
            string sourceImagePath = file.GetLocalFilePath();

            var output = await _service.ContourAsync(sourceImagePath);

            var result = CleanUpFile(sourceImagePath, output);
            _logger.LogInformation("Took {} ms to execute", (DateTime.Now - start).TotalMilliseconds);
            return result;
        }

        [HttpPost("segmentation")]
        public async Task<IActionResult> Segmentation(IFormFile file)
        {
            var start = DateTime.Now;
            string sourceImagePath = file.GetLocalFilePath();
            var output = await _service.SegmentationAsync(sourceImagePath);
            var result = CleanUpFile(sourceImagePath, output);
            _logger.LogInformation("Took {} ms to execute", (DateTime.Now - start).TotalMilliseconds);
            return result;
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
