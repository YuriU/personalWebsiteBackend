using System;
using System.Threading.Tasks;
using personalWebsiteBackend.AppConfiguration;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

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
            
            await Task.Delay(0);
        }
    }
}