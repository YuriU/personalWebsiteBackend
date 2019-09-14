namespace personalWebsiteBackend.AppConfiguration
{
    public class SourceBucketConfiguration
    {
        public SourceBucketConfiguration(string bucketName, string fileName)
        {
            BucketName = bucketName;
            FileName = fileName;
        }

        public readonly string BucketName;

        public readonly string FileName;
    }
}