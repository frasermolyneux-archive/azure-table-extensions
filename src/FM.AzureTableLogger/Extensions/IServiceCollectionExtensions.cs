using System;
using FM.AzureTableLogger.Config;
using Microsoft.Extensions.DependencyInjection;

namespace FM.AzureTableLogger.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        public static void AddAzureTableLogger(this IServiceCollection serviceCollection,
            Action<AzureTableLoggerOptions> options)
        {
            serviceCollection.Configure(options);
            // Add singleton instance that can be injected directly to provide access to health check and clean up functions
            serviceCollection.AddSingleton<AzureTableLogger>();

            serviceCollection.AddLogging(
                logging => { logging.AddAzureTableLogger(); });
        }
    }
}