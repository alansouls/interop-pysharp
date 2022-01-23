using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PythonInteropAPI.Services
{
    public interface IOpenCVService
    {
        /// <summary>
        /// Blurs image according to intensity
        /// </summary>
        /// <param name="image"></param>
        /// <param name="intensity"></param>
        /// <returns>output image location</returns>
        Task<string> BlurAsync(string image, int intensity);

        /// <summary>
        /// Draws contours of an image 
        /// </summary>
        /// <param name="image"></param>
        /// <returns>output image location</returns>
        Task<string> ContourAsync(string image);

        /// <summary>
        /// Draws image segmentations
        /// </summary>
        /// <param name="image"></param>
        /// <returns>output image location</returns>
        Task<string> SegmentationAsync(string image);
    }
}
