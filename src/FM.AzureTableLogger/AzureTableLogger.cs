using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FM.AzureTableLogger.CloudEntities;
using FM.AzureTableLogger.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger
{
    public class AzureTableLogger : IAzureTableLogger
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<AzureTableLoggerOptions> _options;

        public AzureTableLogger(IOptions<AzureTableLoggerOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;

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
            if (!LogsTable.Exists())
                CreateTablesIfNotExist().Wait();

            var username = "anon";
            var requestId = "N/A";

            if (_httpContextAccessor.HttpContext != null)
            {
                requestId = Activity.Current?.Id ?? _httpContextAccessor.HttpContext.TraceIdentifier;

                if (_httpContextAccessor.HttpContext.User != null)
                    username = _httpContextAccessor.HttpContext.User.Identity.Name;
            }
            else
            {
                username = "system";
            }

            var log = new LogEntryCloudEntity(logLevel, eventId, formatter(state, exception), exception, username,
                requestId);

            var insertOp = TableOperation.Insert(log);
            LogsTable.ExecuteAsync(insertOp).GetAwaiter().GetResult();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _options.Value.MinimumLogLevel < logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}