using System;

namespace personalWebsiteBackend.AppConfiguration
{
    public class ConfigurationReader
    {
        public const string SourceBucket_BucketName = "SourceBucket_Name";

        public const string SourceBucket_FileName = "SourceBucket_FileName";

        public const string Database_CounterTable = "Database_CounterTable";

        public const string Database_LogTable = "Database_LogTable";

        public static SourceBucketConfiguration GetSourceBucketConfiguration()
        {
            return new SourceBucketConfiguration(
                Environment.GetEnvironmentVariable(SourceBucket_BucketName),
                Environment.GetEnvironmentVariable(SourceBucket_FileName)
            );
        }

        public static DatabaseConfiguration GetDatabaseConfiguration()
        {
            return new DatabaseConfiguration(
                Environment.GetEnvironmentVariable(Database_CounterTable),
                Environment.GetEnvironmentVariable(Database_LogTable)
            );
        }
    }
}