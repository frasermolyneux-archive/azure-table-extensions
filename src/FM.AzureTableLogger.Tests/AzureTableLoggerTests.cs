using FakeItEasy;
using FluentAssertions;
using FM.AzureTableLogger.Config;
using FM.AzureTableLogger.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace FM.AzureTableLogger.Tests
{
    [TestFixture]
    public class AzureTableLoggerTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeCloudTableClientProviderFactory = A.Fake<ICloudTableClientProviderFactory>();
            _fakeHttpContextAccessor = A.Fake<IHttpContextAccessor>();
        }

        private ICloudTableClientProviderFactory _fakeCloudTableClientProviderFactory;
        private IHttpContextAccessor _fakeHttpContextAccessor;

        [TestCase(LogLevel.Trace, LogLevel.Trace, true)]
        [TestCase(LogLevel.Trace, LogLevel.Debug, true)]
        [TestCase(LogLevel.Trace, LogLevel.Information, true)]
        [TestCase(LogLevel.Trace, LogLevel.Warning, true)]
        [TestCase(LogLevel.Trace, LogLevel.Error, true)]
        [TestCase(LogLevel.Trace, LogLevel.Critical, true)]
        [TestCase(LogLevel.Debug, LogLevel.Trace, false)]
        [TestCase(LogLevel.Debug, LogLevel.Debug, true)]
        [TestCase(LogLevel.Debug, LogLevel.Information, true)]
        [TestCase(LogLevel.Debug, LogLevel.Warning, true)]
        [TestCase(LogLevel.Debug, LogLevel.Error, true)]
        [TestCase(LogLevel.Debug, LogLevel.Critical, true)]
        [TestCase(LogLevel.Information, LogLevel.Trace, false)]
        [TestCase(LogLevel.Information, LogLevel.Debug, false)]
        [TestCase(LogLevel.Information, LogLevel.Information, true)]
        [TestCase(LogLevel.Information, LogLevel.Warning, true)]
        [TestCase(LogLevel.Information, LogLevel.Error, true)]
        [TestCase(LogLevel.Information, LogLevel.Critical, true)]
        [TestCase(LogLevel.Warning, LogLevel.Trace, false)]
        [TestCase(LogLevel.Warning, LogLevel.Debug, false)]
        [TestCase(LogLevel.Warning, LogLevel.Information, false)]
        [TestCase(LogLevel.Warning, LogLevel.Warning, true)]
        [TestCase(LogLevel.Warning, LogLevel.Error, true)]
        [TestCase(LogLevel.Warning, LogLevel.Critical, true)]
        [TestCase(LogLevel.Error, LogLevel.Trace, false)]
        [TestCase(LogLevel.Error, LogLevel.Debug, false)]
        [TestCase(LogLevel.Error, LogLevel.Information, false)]
        [TestCase(LogLevel.Error, LogLevel.Warning, false)]
        [TestCase(LogLevel.Error, LogLevel.Error, true)]
        [TestCase(LogLevel.Error, LogLevel.Critical, true)]
        [TestCase(LogLevel.Critical, LogLevel.Trace, false)]
        [TestCase(LogLevel.Critical, LogLevel.Debug, false)]
        [TestCase(LogLevel.Critical, LogLevel.Information, false)]
        [TestCase(LogLevel.Critical, LogLevel.Warning, false)]
        [TestCase(LogLevel.Critical, LogLevel.Error, false)]
        [TestCase(LogLevel.Critical, LogLevel.Critical, true)]
        public void TestIsEnabled(LogLevel minimumLogLevel, LogLevel testLogLevel, bool expected)
        {
            // Arrange
            var fakeOptions = A.Fake<IOptions<AzureTableLoggerOptions>>();
            A.CallTo(() => fakeOptions.Value).Returns(new AzureTableLoggerOptions
            {
                MinimumLogLevel = minimumLogLevel
            });

            var azureTableLogger =
                new AzureTableLogger(fakeOptions, _fakeHttpContextAccessor, _fakeCloudTableClientProviderFactory);

            // Act
            var result = azureTableLogger.IsEnabled(testLogLevel);

            // Assert
            result.Should().Be(expected);
        }
    }
}