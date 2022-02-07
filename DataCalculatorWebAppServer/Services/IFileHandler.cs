using System.Threading.Tasks;
using DataCalculatorWebAppServer.Models;


namespace DataCalculatorWebAppServer.Services
{
    public interface IFileHandler
    {
        Task UploadFileAsync(FileSendData fileSendData);
    }
}
