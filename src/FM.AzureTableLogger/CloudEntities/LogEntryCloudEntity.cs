using System;
using FM.AzureTableExtensions.Library;
using FM.AzureTableExtensions.Library.Attributes;
using Microsoft.Extensions.Logging;

namespace FM.AzureTableLogger.CloudEntities
{
    public class LogEntryCloudEntity : TableEntityExtended
    {
        public LogEntryCloudEntity(LogLevel logLevel, EventId eventId, string message, Exception exception)
        {
            LogLevel = logLevel;
            EventId = eventId;
            Message = message;
            Exception = exception;

            PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        [EntityEnumPropertyConverter] public LogLevel LogLevel { get; }

        [EntityEnumPropertyConverter] public EventId EventId { get; }

        public string Message { get; }

        [EntityJsonPropertyConverter] public Exception Exception { get; }
    }
}