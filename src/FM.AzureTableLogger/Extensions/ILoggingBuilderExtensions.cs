using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FM.AzureTableLogger.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class ILoggingBuilderExtensions
    {
        public static ILoggingBuilder AddAzureTableLogger(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider, AzureTableLoggerProvider>();
            return loggingBuilder;
        }
    }
}