using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Net;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace personalWebsiteBackend
{
    public class RequestHandler
    {
        
        /// <summary>
        /// Function which returns 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse HandleRequest(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var result = new Dictionary<string, string>();
            result["message"] = "Hello world";
            result["version"] = "2.0";
            string body = JsonConvert.SerializeObject(result);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = body,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" },
                    { "Content-Disposition", "attachment; filename=\"file.json\"" }
                }
            };

            return response;
        }
    }
}
