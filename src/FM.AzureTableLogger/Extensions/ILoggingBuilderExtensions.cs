using System;
using FM.AzureTableLogger.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class ILoggingBuilderExtensions
    {
        public static ILoggingBuilder AddAzureTableLogger(this ILoggingBuilder loggingBuilder,
            IOptions<AzureTableLoggerOptions> configureOptions)
        {
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            loggingBuilder.Services.AddLogging(builder =>
                builder.AddProvider(new AzureTableLoggerProvider(configureOptions)));

            return loggingBuilder;
        }
    }
}