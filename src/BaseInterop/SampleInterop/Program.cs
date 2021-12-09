﻿using BaseInterop.Infrastructure;
using PythonInterop.Infrastructure;
using SampleInterop.Services;
using System;

namespace SampleInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new SampleInterop(new FacialRecognitionService("facial_rec.py", new PythonCodeGenerator(), new DataTransfer()));

            app.Run("image1.jpg", "image2.jpg");
            app.Run("image1.jpg", "image3.jpg");
            app.Run("image2.jpg", "image3.jpg");
        }
    }
}
