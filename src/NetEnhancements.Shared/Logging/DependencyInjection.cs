using System.Reflection;
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
                    // This one must exist.
                    log4NetOptions.Log4NetConfigFileName = "log4net.config";
                }

                logging.AddLog4Net(log4NetOptions);

                logging.AddSimpleConsole(console =>
                {
                    console.IncludeScopes = true;
                    console.SingleLine = true;
                    console.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
                });

                logging.AddDebug();
            });
        }
    }
}
