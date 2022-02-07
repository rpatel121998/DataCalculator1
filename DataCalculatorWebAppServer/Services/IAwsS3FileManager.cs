﻿using System.IO;
using System.Threading.Tasks;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Services
{
    public interface IAwsS3FileManager
    {
        Task<string> UploadFileAsync(string fileName, Stream file);

       
    }
}
