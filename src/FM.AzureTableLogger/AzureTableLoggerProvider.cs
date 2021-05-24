using FM.AzureTableLogger.Config;
using FM.AzureTableLogger.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger
{
    public class AzureTableLoggerProvider : ILoggerProvider
    {
        private readonly ICloudTableClientProviderFactory _cloudTableClientProviderFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<AzureTableLoggerOptions> _options;

        public AzureTableLoggerProvider(
            IOptions<AzureTableLoggerOptions> options,
            IHttpContextAccessor httpContextAccessor,
            ICloudTableClientProviderFactory cloudTableClientProviderFactory)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _cloudTableClientProviderFactory = cloudTableClientProviderFactory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AzureTableLogger(_options, _httpContextAccessor, _cloudTableClientProviderFactory);
        }

        public void Dispose()
        {
        }
    }
}