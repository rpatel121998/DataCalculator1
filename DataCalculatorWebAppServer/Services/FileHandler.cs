using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var s3fileName = await _s3FileManager.UploadFileAsync(fileSendData.File.Name, fileSendData.File.Data);
            await _context.SaveChangesAsync();
        }

        public async Task<TransferFile> DownloadFileAsync(string fileName)
        {
            var dbFileData = await _context
                .FileStorageData.Include(f => f.FileSendData).SingleOrDefaultAsync(f => f.FileName == fileName);

            if (dbFileData == null)
            {
                return new TransferFile();
            }

            var file = await _s3FileManager.DownloadFileAsync(fileName);

            _context.FileSendData.Remove(dbFileData.FileSendData);
            _context.FileStorageData.Remove(dbFileData);
            await _context.SaveChangesAsync();

            return file;
        }
    }
}
