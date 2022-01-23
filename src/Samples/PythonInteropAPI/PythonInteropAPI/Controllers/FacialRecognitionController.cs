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
        public FacialRecognitionController(IFacialRecognitionService service)
        {
            _service = service;
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Blur(IFormFile face1, IFormFile face2)
        {
            string sourceImage1Path = face1.GetLocalFilePath();
            string sourceImage2Path = face2.GetLocalFilePath();

            var output = await _service.CheckFacesAsync(sourceImage1Path, sourceImage2Path);
            
            System.IO.File.Delete(sourceImage1Path);
            System.IO.File.Delete(sourceImage2Path);

            return Ok(new { sameFaces = output });
        }
    }
}
