using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetEnhancements.Shared.Logging;

namespace NetEnhancements.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = CreateDefaultBuilder(args);

            var loggingTest = host.Services.GetRequiredService<LoggingTest>();
            loggingTest.Start();

            await host.RunAsync();
        }
        private static IHost CreateDefaultBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices((context, collection) =>
                {
                    collection.AddLogging(context.Configuration);
                    collection.AddScoped<LoggingTest>();
                }).Build();
        }
    }
}