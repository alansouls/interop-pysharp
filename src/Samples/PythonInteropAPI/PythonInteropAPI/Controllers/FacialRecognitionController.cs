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
    public class FacialRecognitionController : ControllerBase
    {
        private readonly IFacialRecognitionService _service;
        private readonly ILogger<FacialRecognitionController> _logger;
        public FacialRecognitionController(IFacialRecognitionService service,
            ILogger<FacialRecognitionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Blur(IFormFile face1, IFormFile face2)
        {
            var start = DateTime.Now;
            string sourceImage1Path = face1.GetLocalFilePath();
            string sourceImage2Path = face2.GetLocalFilePath();

            var output = await _service.CheckFacesAsync(sourceImage1Path, sourceImage2Path);
            
            System.IO.File.Delete(sourceImage1Path);
            System.IO.File.Delete(sourceImage2Path);

            _logger.LogInformation("Took {} ms", (DateTime.Now - start).TotalMilliseconds);
            return Ok(new { sameFaces = output });
        }
    }
}
