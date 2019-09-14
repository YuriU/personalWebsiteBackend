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
using System.IO;
using Amazon;
using personalWebsiteBackend.AppConfiguration;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace personalWebsiteBackend
{
    public class RequestHandler
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUCentral1; 

        private static IAmazonS3 client;

        private readonly SourceBucketConfiguration sourceBucketConfiguration;

        public RequestHandler() 
        {
            sourceBucketConfiguration = ConfigurationReader.GetSourceBucketConfiguration();
            client = new AmazonS3Client(bucketRegion);
        }

        /// <summary>
        /// Function which returns 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> HandleRequest(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return await ReturnDownloadedFile();

            var buckets = await client.ListObjectsAsync(sourceBucketConfiguration.BucketName, "*");
            /*var result = new Dictionary<string, string>();
            result["message"] = "Hello world";
            result["version"] = "2.0";
             */

            string body = JsonConvert.SerializeObject(buckets);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = body,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" },
                    //{ "Content-Disposition", "attachment; filename=\"file.json\"" }
                }
            };

            return response;
        }

        public async Task<APIGatewayProxyResponse> ReturnDownloadedFile()
        {
            var request = new GetObjectRequest
            {
                BucketName = sourceBucketConfiguration.BucketName,
                Key = sourceBucketConfiguration.FileName,
            };

            using (GetObjectResponse getObjectResponse = await client.GetObjectAsync(request))
            {
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = ConvertToBase64(getObjectResponse.ResponseStream),
                    Headers = new Dictionary<string, string>
                    { 
                        { "Content-Type", "application/msword" }, 
                        { "Access-Control-Allow-Origin", "*" },
                        { "Content-Disposition", $"attachment; filename=\"{sourceBucketConfiguration.FileName}\"" }
                    },
                    IsBase64Encoded = true
                };

                return response;
            }
        }

        public static string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
    }
}
