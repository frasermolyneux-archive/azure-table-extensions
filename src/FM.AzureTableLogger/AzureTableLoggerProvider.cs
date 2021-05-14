using FM.AzureTableLogger.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FM.AzureTableLogger
{
    public class AzureTableLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<AzureTableLoggerOptions> _options;

        public AzureTableLoggerProvider(IOptions<AzureTableLoggerOptions> options,
            IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AzureTableLogger(_options, _httpContextAccessor);
        }

        public void Dispose()
        {
        }
    }
}