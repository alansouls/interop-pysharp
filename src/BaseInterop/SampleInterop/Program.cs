using SampleInterop.Services;
using System;

namespace SampleInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new SampleInterop(new FacialRecognitionService("facial_rec.py", null, null));

            app.Run("image1.jpg", "image2.jpg");
        }
    }
}
