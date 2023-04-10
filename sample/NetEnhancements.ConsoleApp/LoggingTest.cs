using Microsoft.Extensions.Logging;

namespace NetEnhancements.ConsoleApp
{
    internal class LoggingTest
    {
        private readonly ILogger<LoggingTest> _logger;

        public LoggingTest(ILogger<LoggingTest> logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            _logger.LogTrace("Tracing...");
            _logger.LogDebug("Debuging...");
            _logger.LogInformation("Infoing...");
            _logger.LogWarning("Warning...");
            _logger.LogError("Erroring...");
            _logger.LogCritical("Criticaling...");
        }
    }
}
