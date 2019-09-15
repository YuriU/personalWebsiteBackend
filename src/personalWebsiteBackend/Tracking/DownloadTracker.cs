using System;
using System.Threading.Tasks;
using personalWebsiteBackend.AppConfiguration;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace personalWebsiteBackend.Utils
{
    public class DownloadTracker : IDownloadTracker 
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        private readonly DatabaseConfiguration _databaseConfiguration;

        public DownloadTracker(IAmazonDynamoDB dynamoDbClient, DatabaseConfiguration databaseConfiguration)
        {
            _dynamoDbClient = dynamoDbClient;
            _databaseConfiguration = databaseConfiguration;
        }

        public async Task TrackDownload(string fileName)
        {
            await UpdateDownloadedCounter(fileName);
        }

        public async Task UpdateDownloadedCounter(string fileName)
        {
            var request = new UpdateItemRequest
            {
                TableName = _databaseConfiguration.DownloadCounterTable,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "FileName", new AttributeValue { S = fileName } }
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>()
                {
                    {
                        "Downloaded",
                        new AttributeValueUpdate { Action = "ADD", Value = new AttributeValue { N = "1" } }
                    },
                },
            };

            await _dynamoDbClient.UpdateItemAsync(request);
        }
    }
}