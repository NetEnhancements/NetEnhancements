using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace NetEnhancements.Services
{
    /// <summary>
    /// A <see cref="BackgroundService"/> that calls <see cref="ExecuteScheduledTaskAsync"/> based on the <see cref="CronSchedule"/>.
    /// </summary>
    public abstract class ScheduledBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly Lazy<CrontabSchedule> _crontabSchedule;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<ScheduledBackgroundService> Logger;

        /// <summary>
        /// When the service starts, it will by default run <see cref="ExecuteAsync"/> once.
        /// Set this property to <c>true</c> in the constructor to skip that initial run on service startup, so it only runs on the scheduled time.
        /// </summary>
        protected bool SkipInitialRun = false;

        /// <summary>
        /// The cron schedule pattern, including seconds.
        /// </summary>
        protected abstract string CronSchedule { get; }

        /// <summary>
        /// Insantiates the service.
        /// </summary>
        protected ScheduledBackgroundService(ILogger<ScheduledBackgroundService> logger, IServiceProvider services)
        {
            Logger = logger;
            _services = services;

            _crontabSchedule = new Lazy<CrontabSchedule>(() =>
            {
                var crontabSchedule = CrontabSchedule.TryParse(CronSchedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });

                return crontabSchedule ?? throw new InvalidOperationException($"Could not parse cron schedule '{CronSchedule}'");
            });
        }

        private TimeSpan GetDelay()
        {
            var now = DateTime.Now;
            
            var nextOccurrence = _crontabSchedule.Value.GetNextOccurrence(now, now.AddMonths(3));

            return nextOccurrence - now;
        }

        /// <inheritdoc />
        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Scheduled service {serviceName} running on schedule {schedule}", GetType().FullName, CronSchedule);

            var isInitialRun = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!isInitialRun || !SkipInitialRun)
                    {
                        using var scope = _services.CreateScope();
                        
                        await ExecuteScheduledTaskAsync(scope.ServiceProvider, stoppingToken);
                    }

                    isInitialRun = false;

                    await Task.Delay(GetDelay(), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    Logger.LogInformation("Task cancelled, scheduled service {serviceName} stopping", GetType().FullName);

                    return;
                }
            }
        }

        /// <summary>
        /// Implement this method instead of <see cref="ExecuteAsync"/> to do your work.
        /// </summary>
        protected abstract Task ExecuteScheduledTaskAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken);
    }
}
