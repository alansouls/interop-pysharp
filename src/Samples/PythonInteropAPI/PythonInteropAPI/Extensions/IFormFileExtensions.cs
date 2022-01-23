using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PythonInteropAPI.Extensions
{
    public static class IFormFileExtensions
    {
        public static string GetLocalFilePath(this IFormFile file)
        {
            var sourceImagePath = Path.Combine("images", Guid.NewGuid().ToString(), file.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(sourceImagePath));
            using (var fileStream = new FileStream(sourceImagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return sourceImagePath;
        }
    }
}
