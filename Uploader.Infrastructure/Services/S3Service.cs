using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Uploader.Application.Images.Services;
using Uploader.Infrastructure.Config;

namespace Uploader.Infrastructure.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Settings _settings;

        public S3Service(IAmazonS3 s3Client, S3Settings settings)
        {
            _s3Client = s3Client;
            _settings = settings;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var key = Guid.NewGuid() + Path.GetExtension(file.FileName);

            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    BucketName = _settings.BucketName,
                    Key = key,
                    CannedACL = S3CannedACL.PublicRead
                };
                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return GetUploadedUrl(key);
        }

        private string GetUploadedUrl(string key)
        {
            return $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{key}";
        }
    }
}