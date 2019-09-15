using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using personalWebsiteBackend.Utils;

namespace personalWebsiteBackend.Download
{
    public class S3FileDownloader : IFileDownloader
    {
        private readonly IAmazonS3 _s3Client;

        public S3FileDownloader(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<string> DownloadBase64String(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
            };

            using (GetObjectResponse getObjectResponse = await _s3Client.GetObjectAsync(request))
            {
                return getObjectResponse.ResponseStream.ToBase64String();
            }
        }
    }
}