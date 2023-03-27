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
        protected readonly ILogger<ScheduledBackgroundService> Logger;
        private readonly IServiceProvider _services;

        protected abstract string CronSchedule { get; }

        protected ScheduledBackgroundService(ILogger<ScheduledBackgroundService> logger, IServiceProvider services)
        {
            Logger = logger;
            _services = services;
        }

        private TimeSpan GetDelay()
        {
            var now = DateTime.Now;
            var crontabSchedule = CrontabSchedule.TryParse(CronSchedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            var nextOccurrence = crontabSchedule.GetNextOccurrence(now, now.AddMonths(3));

            return nextOccurrence - now;
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Scheduled service {serviceName} running on schedule {schedule}", GetType().FullName, CronSchedule);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        await ExecuteScheduledTaskAsync(scope.ServiceProvider, stoppingToken);
                    }

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
