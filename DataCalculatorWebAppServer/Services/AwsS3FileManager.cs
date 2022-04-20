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
    // The actual controller where the S3 upload happens
    public class AwsS3FileManager : IAwsS3FileManager 
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucket;

        public AwsS3FileManager(IAmazonS3 client)
        {
            _client = client;
            _bucket = "seconddatacalc";
        }

        public async Task<string> UploadFileAsync(string fileName, Stream file, string bucketName)
        {
            var filestream = new MemoryStream();
            await file.CopyToAsync(filestream);

            var s3FileName = $"{fileName}";

            var transferRequest = new TransferUtilityUploadRequest()
            {
                ContentType = "application/json",
                InputStream = filestream,
                BucketName = bucketName,
                Key = s3FileName
            };
            transferRequest.Metadata.Add("x-amz-meta-title", fileName);

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(transferRequest);

            return s3FileName;
        }
        public async Task<TransferFile> DownloadFileAsync(string fileName, string bucketName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            using (var objectResponse = await _client.GetObjectAsync(request))
            {
                if (objectResponse.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Could not find file.");
                }

                using (var responseStream = objectResponse.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var result = new MemoryStream();
                    responseStream.CopyTo(result);
                    return new TransferFile
                    {
                        Name = fileName,
                        Content = result.ToArray()
                    };
                }
            }
        }

        public async Task<List<S3Object>> ListObjectAsync(string bucketName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = "write/"
            };
            var response = await _client.ListObjectsV2Async(request);
            return response.S3Objects;
        }
    }
}
