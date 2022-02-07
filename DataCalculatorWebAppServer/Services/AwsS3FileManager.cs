using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DataCalculatorWebAppServer.Models; 

namespace DataCalculatorWebAppServer.Services
{
    public class AwsS3FileManager : IAwsS3FileManager
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucket;

        public AwsS3FileManager(IAmazonS3 client)
        {
            _client = client;
            _bucket = "rawjsondatanvd";
        }

        public async Task<string> UploadFileAsync(string fileName, Stream file)
        {
            var filestream = new MemoryStream();
            await file.CopyToAsync(filestream);

            var s3FileName = $"{DateTime.Now.Ticks}-{fileName}";

            var transferRequest = new TransferUtilityUploadRequest()
            {
                ContentType = "application/json",
                InputStream = filestream,
                BucketName = _bucket,
                Key = s3FileName
            };
            transferRequest.Metadata.Add("x-amz-meta-title", fileName);

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(transferRequest);

            return s3FileName;
        }
    }
}
