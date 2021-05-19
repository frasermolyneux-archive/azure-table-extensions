using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FM.AzureTableLogger.CloudEntities;
using FM.AzureTableLogger.Config;
using FM.AzureTableLogger.Factories;
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

        private readonly Dictionary<LogLevel, int> _logLevels = new Dictionary<LogLevel, int>
        {
            {LogLevel.Trace, 1},
            {LogLevel.Debug, 2},
            {LogLevel.Information, 3},
            {LogLevel.Warning, 4},
            {LogLevel.Error, 6},
            {LogLevel.Critical, 7},
            {LogLevel.None, 8}
        };

        private readonly IOptions<AzureTableLoggerOptions> _options;

        public AzureTableLogger(
            IOptions<AzureTableLoggerOptions> options,
            IHttpContextAccessor httpContextAccessor,
            ICloudTableClientProviderFactory cloudTableClientProviderFactory)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;

            var cloudTableClientProvider = cloudTableClientProviderFactory.Create(options);
            _cloudTableClient = cloudTableClientProvider.GetTableClient();
            LogsTable = cloudTableClientProvider.GetCloudTable(options.Value.LogsTableName);
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

            if (!IsEnabled(logLevel))
                return;

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
            var minimumLogLevel = _logLevels[_options.Value.MinimumLogLevel];
            var checkLogLevel = _logLevels[logLevel];

            return checkLogLevel >= minimumLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}