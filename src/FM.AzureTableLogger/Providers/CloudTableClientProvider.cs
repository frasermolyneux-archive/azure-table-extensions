using FM.AzureTableLogger.Config;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger.Providers
{
    public class CloudTableClientProvider : ICloudTableClientProvider
    {
        private readonly CloudTableClient _cloudTableClient;

        public CloudTableClientProvider(IOptions<AzureTableLoggerOptions> options)
        {
            var storageAccount = CloudStorageAccount.Parse(options.Value.StorageConnectionString);
            _cloudTableClient = storageAccount.CreateCloudTableClient();
        }

        public CloudTableClient GetTableClient()
        {
            return _cloudTableClient;
        }

        public CloudTable GetCloudTable(string tableName)
        {
            return _cloudTableClient.GetTableReference(tableName);
        }
    }
}