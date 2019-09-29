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
        private readonly IFileDownloader _downloader;

        //private readonly IDownloadTracker _downloadTracker;

        private readonly SourceBucketConfiguration _sourceBucketConfiguration;

        public RequestHandler(IFileDownloader downloader, IDownloadTracker downloadTracker, SourceBucketConfiguration sourceBucketConfiguration)
        {
            _downloader = downloader;
            _downloadTracker = downloadTracker;
            _sourceBucketConfiguration = sourceBucketConfiguration;
        }
        
        public RequestHandler() 
        {
            _sourceBucketConfiguration = ConfigurationReader.GetSourceBucketConfiguration();
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
            var cmd = request.QueryStringParameters["cmd"];

            if(cmd == "echo")
            {
                // Return request as we get it
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(request),
                    Headers = new Dictionary<string, string>
                    { 
                        { "Content-Type", "application/json" }, 
                        { "Access-Control-Allow-Origin", "*" }
                    },
                };
            }
            else if(cmd == "requestDownload" && request.HttpMethod == "POST")
            {
                return await ReturnDownloadedFile(_sourceBucketConfiguration.BucketName, _sourceBucketConfiguration.FileName);
            }
            else
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = "Invalid request",
                    Headers = new Dictionary<string, string>
                    { 
                        { "Content-Type", "text/plain" }, 
                        { "Access-Control-Allow-Origin", "*" }
                    }
                };
            }
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
