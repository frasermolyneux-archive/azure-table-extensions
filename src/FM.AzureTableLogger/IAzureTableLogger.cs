using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace FM.AzureTableLogger
{
    public interface IAzureTableLogger : ILogger
    {
        CloudTable LogsTable { get; }
        Task CreateTablesIfNotExist();
        Task<Tuple<bool, string>> HealthCheck();
    }
}