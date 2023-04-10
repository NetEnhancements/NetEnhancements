using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

using ILogger = Microsoft.Extensions.Logging.ILogger;

using NetEnhancements.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetEnhancements.Shared.Logging
{
    /// <summary>
    /// Dependency Injection extensions container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Configures Log4Net, Console and Debug logging.
        /// 
        /// https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            AddLogging(services, configuration, new LoggingSettings());
        }

        public static void AddLogging(this IServiceCollection services, IConfiguration configuration, Action<LoggingSettings> loggingAction)
        {
            var logSettings = new LoggingSettings();
            loggingAction.Invoke(logSettings);
            AddLogging(services, configuration, logSettings);
        }

        private static void AddLogging(
            this IServiceCollection services,
            IConfiguration configuration,
            LoggingSettings loggingSettings)
        {
            services.AddLogging(logging =>
                {
                    // Initialize MS Logging
                    var (loggingSection, _) = configuration.GetSectionOrThrow<LoggingSettings>();
                    logging.AddConfiguration(loggingSection);

                    // Add log4Net from configuration file(s).
                    var log4NetOptions = new Log4NetProviderOptions();
                    configuration.Bind(LoggingSettings.Log4NetSectionName, log4NetOptions);

                    var codeBase = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
                    if (codeBase == null)
                    {
                        throw new ArgumentException($"{nameof(AddLogging)} failed: GetExecutingAssembly().Location not found!");
                    }

                    var logConfigFile = Path.Combine(codeBase.FullName, log4NetOptions.Log4NetConfigFileName);

                    if (!File.Exists(logConfigFile))
                    {
                        log4NetOptions.Log4NetConfigFileName = "log4net.config";
                    }

                    logging.BuildLog4NetRepository(log4NetOptions, loggingSettings);

                    logging.AddSimpleConsole(console =>
                        {
                            console.IncludeScopes = true;
                            console.SingleLine = true;
                            console.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
                        });

                    logging.AddDebug();
                });
        }

        private static void BuildLog4NetRepository(this ILoggingBuilder logging, Log4NetProviderOptions options, LoggingSettings loggingSettings)
        {
            if (!string.IsNullOrEmpty(options.Log4NetConfigFileName) && File.Exists(options.Log4NetConfigFileName))
            {
                logging.AddLog4Net(options);
            }
            else
            {
                ILoggerRepository repository;
                try
                {
                    repository = LogManager.GetRepository("HouseOfTickets.Logging.Repository");

                }
                catch (LogException _)
                {
                    repository = LogManager.CreateRepository("HouseOfTickets.Logging.Repository");
                }

                ConfigureLog4Net(repository, loggingSettings);
                logging.Services.AddSingleton(repository);
                logging.Services.AddSingleton<ILoggerProvider, LoggingRepository>();
            }
        }

        private static void ConfigureLog4Net(ILoggerRepository repository, LoggingSettings loggingSettings)
        {
            var hierarchy = (Hierarchy)repository;

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger [%property{NDC}] - %message%newline"
            };

            patternLayout.ActivateOptions();

            var assembly = Assembly.GetEntryAssembly();

            var roller = new RollingFileAppender
            {
                Name = "RollingFile",
                File = @$"{loggingSettings.LogLocation ?? "Logs"}\{assembly?.GetName().Name ?? Process.GetCurrentProcess().ProcessName}",
                DatePattern = "'.'yyyy-MM-dd'.log'",
                StaticLogFileName = false,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "1MB",
                LockingModel = new FileAppender.MinimalLock(),
                Layout = patternLayout
            };
            roller.ActivateOptions();

            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.Warn;
            hierarchy.Configured = true;
        }
    }

    internal class LoggingRepository : ILoggerProvider
    {
        #region Implementation of IDisposable

        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();
        private readonly ILoggerRepository _loggerRepository;
        private bool _disposedValue = false;

        public LoggingRepository()
        {
            _loggerRepository = LogManager.GetRepository("HouseOfTickets.Logging.Repository");
        }

        private IExternalScopeProvider ExternalScopeProvider { get; set; } = new LoggerExternalScopeProvider();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _loggerRepository.Shutdown();
                    _loggers.Clear();
                }

                _disposedValue = true;
            }
        }

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(
                new Log4NetProviderOptions()
                {
                    Name = name,
                    LoggerRepository = _loggerRepository.Name,
                    LoggingEventFactory = new Log4NetLoggingEventFactory(),
                    LogLevelTranslator = new Log4NetLogLevelTranslator()
                },
                ExternalScopeProvider);
        }

        #endregion
    }
}
