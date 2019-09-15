using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using personalWebsiteBackend;

namespace personalWebsiteBackend.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestHandleRequest()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var requestHandler = new RequestHandler();
            var context = new TestLambdaContext();
            var response = await requestHandler.HandleRequest(new APIGatewayProxyRequest(), context);

            Assert.Equal("{\"message\":\"Hello world\"}", response.Body);
        }
    }
}
