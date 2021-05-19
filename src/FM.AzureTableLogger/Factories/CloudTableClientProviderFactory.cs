using FM.AzureTableLogger.Config;
using FM.AzureTableLogger.Providers;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger.Factories
{
    public class CloudTableClientProviderFactory : ICloudTableClientProviderFactory
    {
        public ICloudTableClientProvider Create(IOptions<AzureTableLoggerOptions> options)
        {
            return new CloudTableClientProvider(options);
        }
    }
}