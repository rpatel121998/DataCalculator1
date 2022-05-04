using System.Threading.Tasks;
using DataCalculatorWebAppServer.Models;
using Amazon.S3.Model;


namespace DataCalculatorWebAppServer.Services
{
    public interface IFileHandler
    {
        Task UploadFileAsync(FileSendData fileSendData, string bucketName);

        Task<TransferFile> DownloadFileAsync(string fileName, string bucketName);

        Task<List<S3Object>> ListObjectAsync(string bucketName);

    }
}
