namespace personalWebsiteBackend.AppConfiguration
{
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration(string downloadCounterTable, string logTable)
        {
            DownloadCounterTable = downloadCounterTable;
            LogTable = logTable;
        }

        public readonly string DownloadCounterTable;

        public readonly string LogTable;
    }
}