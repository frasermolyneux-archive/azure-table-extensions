// ReSharper disable InconsistentNaming

using Microsoft.AspNetCore.Builder;

namespace FM.AzureTableLogger.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseAzureTableLogging(this IApplicationBuilder app)
        {
            ((IAzureTableLogger) app.ApplicationServices.GetService(typeof(IAzureTableLogger)))
                ?.CreateTablesIfNotExist().Wait();
        }
    }
}