using FM.AzureTableLogger.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger
{
    public class AzureTableLoggerProvider : ILoggerProvider
    {
        private readonly IOptions<AzureTableLoggerOptions> _options;

        public AzureTableLoggerProvider(IOptions<AzureTableLoggerOptions> options)
        {
            _options = options;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AzureTableLogger(_options);
        }

        public void Dispose()
        {
        }
    }
}