using Microsoft.Extensions.Logging;

namespace FM.AzureTableLogger.Config
{
    public class AzureTableLoggerOptions
    {
        public string StorageConnectionString { get; set; }
        public string LogsTableName { get; set; }
        public LogLevel MinimumLogLevel { get; set; }
    }
}