using DataCalculatorWebAppServer.Data;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly DataCalculatorDbContext _context;
        private readonly IAwsS3FileManager _s3FileManager;

        public FileHandler(
            DataCalculatorDbContext context,
            IAwsS3FileManager s3FileManager)
        {
            _context = context;
            _s3FileManager = s3FileManager;
        }

        public async Task UploadFileAsync(FileSendData fileSendData)
        {
            _context.FileSendData.Add(fileSendData);

            var s3fileName = await _s3FileManager.UploadFileAsync(fileSendData.File.Name, fileSendData.File.Data);

            _context.FileStorageData.Add(new FileStorageData
            {
                FileSendDataId = fileSendData.Id,
                FileUri = s3fileName
            });
            await _context.SaveChangesAsync();
        }
    }
}
