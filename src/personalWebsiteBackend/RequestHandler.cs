using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Net;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using personalWebsiteBackend.AppConfiguration;
using personalWebsiteBackend.Utils;
using personalWebsiteBackend.Download;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace personalWebsiteBackend
{
    public class RequestHandler
    {        
        private IFileDownloader _downloader;

        private IDownloadTracker _downloadTracker;

        private SourceBucketConfiguration sourceBucketConfiguration;

        public RequestHandler() 
        {
            sourceBucketConfiguration = ConfigurationReader.GetSourceBucketConfiguration();
            _downloader = new S3FileDownloader(new AmazonS3Client());
            _downloadTracker = new DownloadTracker(new AmazonDynamoDBClient(), ConfigurationReader.GetDatabaseConfiguration());
        }

        /// <summary>
        /// Function which returns 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> HandleRequest(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return await ReturnDownloadedFile(sourceBucketConfiguration.BucketName, sourceBucketConfiguration.FileName);
        }

        public async Task<APIGatewayProxyResponse> ReturnDownloadedFile(string bucketName, string fileName)
        {
            var fileContent = await _downloader.DownloadBase64String(bucketName, fileName);

            await _downloadTracker.TrackDownload(fileName);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = fileContent,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/msword" }, 
                    { "Access-Control-Allow-Origin", "*" },
                    { "Content-Disposition", $"attachment; filename=\"{fileName}\"" }
                },
                IsBase64Encoded = true
            };
        }
    }
}
