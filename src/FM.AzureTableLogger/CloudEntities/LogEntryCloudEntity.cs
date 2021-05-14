using System;
using FM.AzureTableExtensions.Library;
using FM.AzureTableExtensions.Library.Attributes;
using Microsoft.Extensions.Logging;

namespace FM.AzureTableLogger.CloudEntities
{
    public class LogEntryCloudEntity : TableEntityExtended
    {
        public LogEntryCloudEntity(LogLevel logLevel, EventId eventId, string message, Exception exception,
            string username, string requestId)
        {
            LogLevel = logLevel;
            EventId = eventId;
            Message = message;
            Exception = exception;
            Username = username;
            RequestId = requestId;

            PartitionKey = DateTime.UtcNow.ToString("yyyy-MM");
            YearMonthDay = DateTime.UtcNow.ToString("yyyy-MM-dd");
            RowKey = Guid.NewGuid().ToString();
        }

        [EntityEnumPropertyConverter] public LogLevel LogLevel { get; }

        [EntityEnumPropertyConverter] public EventId EventId { get; }

        public string Message { get; set; }

        public string YearMonthDay { get; set; }
        public string Username { get; set; }
        public string RequestId { get; set; }


        [EntityJsonPropertyConverter] public Exception Exception { get; }
    }
}