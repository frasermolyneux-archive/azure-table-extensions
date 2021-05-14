using System;
using System.Threading.Tasks;
using FM.AzureTableLogger.CloudEntities;
using FM.AzureTableLogger.Config;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger
{
    public class AzureTableLogger : IAzureTableLogger
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly IOptions<AzureTableLoggerOptions> _options;

        public AzureTableLogger(IOptions<AzureTableLoggerOptions> options)
        {
            _options = options;

            var storageAccount = CloudStorageAccount.Parse(options.Value.StorageConnectionString);
            _cloudTableClient = storageAccount.CreateCloudTableClient();

            LogsTable = _cloudTableClient.GetTableReference(options.Value.LogsTableName);
        }

        public CloudTable LogsTable { get; }

        public async Task CreateTablesIfNotExist()
        {
            await LogsTable.CreateIfNotExistsAsync();
        }

        public async Task<Tuple<bool, string>> HealthCheck()
        {
            try
            {
                _ = await _cloudTableClient.GetServicePropertiesAsync();
                return new Tuple<bool, string>(true, "OK");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var log = new LogEntryCloudEntity(logLevel, eventId, formatter(state, exception), exception);

            var insertOp = TableOperation.Insert(log);
            LogsTable.ExecuteAsync(insertOp).GetAwaiter().GetResult();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _options.Value.MinimumLogLevel > logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}