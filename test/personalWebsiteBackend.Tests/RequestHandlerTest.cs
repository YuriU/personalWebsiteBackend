using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Moq;
using personalWebsiteBackend;
using personalWebsiteBackend.AppConfiguration;
using personalWebsiteBackend.Download;
using personalWebsiteBackend.Utils;

namespace personalWebsiteBackend.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestHandleRequest()
        {
            var downloader = Mock.Of<IFileDownloader>();
            var tracker = Mock.Of<IDownloadTracker>();
            var config = new SourceBucketConfiguration("bucket", "file.tf");

            Mock.Get(downloader)
                .Setup(m => m.DownloadBase64String(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult("Response"));
            
            // Invoke the lambda function and confirm the string was upper cased.
            var requestHandler = new RequestHandler(downloader, tracker, config);
            var context = new TestLambdaContext();
            var response = await requestHandler.HandleRequest(new APIGatewayProxyRequest()
            {
                QueryStringParameters = new Dictionary<string, string>() { {"cmd", "requestDownload" }},
                HttpMethod = "POST"
            }, context);

            Assert.Equal("Response", response.Body);
        }
    }
}
