using Microsoft.Extensions.Logging;

namespace NetEnhancements.Services.Tests.Services
{
    /// <summary>
    /// Skips the initial run, then waits 59 seconds.
    /// </summary>
    internal class SkippingService : ScheduledBackgroundService
    {
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task HasStarted => _started.Task;

        public SkippingService(ILogger<ScheduledBackgroundService> logger, IServiceProvider services, string cronSchedule, bool skipInitial)
            : base(logger, services)
        {
            SkipInitialRun = skipInitial;

            CronSchedule = cronSchedule;
        }

        protected override string CronSchedule { get; }

        protected override Task ExecuteScheduledTaskAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            Logger.LogInformation("Doing scheduled work");

            // Signal tests that we've started.
            _started.TrySetResult();

            return Task.CompletedTask;
        }
    }
}
