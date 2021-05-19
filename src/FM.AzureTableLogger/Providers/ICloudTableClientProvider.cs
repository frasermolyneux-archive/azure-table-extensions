using Microsoft.Azure.Cosmos.Table;

namespace FM.AzureTableLogger.Providers
{
    public interface ICloudTableClientProvider
    {
        CloudTableClient GetTableClient();
        CloudTable GetCloudTable(string tableName);
    }
}