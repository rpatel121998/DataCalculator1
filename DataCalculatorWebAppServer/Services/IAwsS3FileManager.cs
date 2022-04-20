using System.IO;
using System.Threading.Tasks;
using Amazon.S3.Model;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Services
{
    public interface IAwsS3FileManager
    {
        Task<string> UploadFileAsync(string fileName, Stream file, string bucketName);
        Task<TransferFile> DownloadFileAsync(string fileName, string bucketName);

        Task<List<S3Object>> ListObjectAsync(string bucketName);

    }
}
