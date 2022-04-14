using System.Threading.Tasks;
using Amazon.S3.Model;
using DataCalculatorWebAppServer.Models;


namespace DataCalculatorWebAppServer.Services
{
    public interface IFileHandler
    {
        Task UploadFileAsync(FileSendData fileSendData);

        Task<TransferFile> DownloadFileAsync(string fileName);

        Task<List<S3Object>> ListObjectAsync(string bucketName);
    }
}
