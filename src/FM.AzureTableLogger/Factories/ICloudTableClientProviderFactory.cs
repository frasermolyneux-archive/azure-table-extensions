using FM.AzureTableLogger.Config;
using FM.AzureTableLogger.Providers;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger.Factories
{
    public interface ICloudTableClientProviderFactory
    {
        ICloudTableClientProvider Create(IOptions<AzureTableLoggerOptions> options);
    }
}