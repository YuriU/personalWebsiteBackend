using System;

namespace personalWebsiteBackend.AppConfiguration
{
    public class ConfigurationReader
    {
        public const string SourceBucket_BucketName = "SourceBucket_Name";

        public const string SourceBucket_FileName = "SourceBucket_FileName";

        public static SourceBucketConfiguration GetSourceBucketConfiguration()
        {
            return new SourceBucketConfiguration(
                Environment.GetEnvironmentVariable(SourceBucket_BucketName),
                Environment.GetEnvironmentVariable(SourceBucket_FileName)
            );
        }
    }
}