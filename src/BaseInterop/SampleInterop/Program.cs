using BaseInterop.Infrastructure;
using PythonInterop.Infrastructure;
using SampleInterop.Services;
using System;

namespace SampleInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            using var app = new SampleInterop(new FacialRecognitionService(new PythonCodeGenerator(), new DataTransfer()));

            app.Run("image1.jpg", "image2.jpg");
            app.Run("image1.jpg", "image3.jpg");
            app.Run("image2.jpg", "image3.jpg");
        }
    }
}
