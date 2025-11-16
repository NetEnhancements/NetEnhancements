using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEnhancements.Services.Tests.Services;
using NSubstitute;

namespace NetEnhancements.Services.Tests
{
    public class SkippingScheduledBackgroundServiceTests
    {
        /// <summary>
        /// TODO: doesn't any other package offer this?
        /// </summary>
        /// <returns></returns>
        private static ServiceContainer BuildScopedContainer()
        {
            var containerFake = new ServiceContainer();

            var scopeMock = Substitute.For<IServiceScope>();
            scopeMock.ServiceProvider.Returns(containerFake);

            var scopeFactoryMock = Substitute.For<IServiceScopeFactory>();
            scopeFactoryMock.CreateScope().Returns(scopeMock);

            containerFake.AddService(typeof(IServiceScopeFactory), scopeFactoryMock);

            return containerFake;
        }

        [Test]
        public async Task InvalidSchedule_Throws()
        {
            // Arrange
            // Invalid because seconds are mandatory.
            const string invalidPattern = "* * * * *";

            var loggerMock = Substitute.For<ILogger<SkippingService>>();
            var classUnderTest = new SkippingService(loggerMock, BuildScopedContainer(), invalidPattern, skipInitial: true);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);

            do
            {
                await Task.Delay(12);
            } while (!classUnderTest.ExecuteTask!.IsCompleted);

            // Assert
            Assert.That(classUnderTest.ExecuteTask!.Exception, Is.InstanceOf<AggregateException>());
            Assert.That(classUnderTest.ExecuteTask!.Exception!.InnerException!, Is.InstanceOf<InvalidOperationException>());
            Assert.That(classUnderTest.ExecuteTask!.Exception!.InnerException!.Message, Contains.Substring("cron schedule"));
        }

        [Test]
        public async Task NotSkippingInitialRun_RunsImmediately()
        {
            // Arrange
            var loggerMock = Substitute.For<ILogger<SkippingService>>();
            var classUnderTest = new SkippingService(loggerMock, BuildScopedContainer(), "*/1 * * * * *", skipInitial: false);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);

            // Assert
            await classUnderTest.HasStarted;
            await cancellationTokenSource.CancelAsync();
        }

        [Test]
        public async Task SkippingInitialRun_WaitsImmediately()
        {
            // Arrange
            var loggerMock = Substitute.For<ILogger<SkippingService>>();
            var classUnderTest = new SkippingService(loggerMock, BuildScopedContainer(), "*/1 * * * * *", skipInitial: true);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);

            // Assert
            await classUnderTest.HasStarted;
            await cancellationTokenSource.CancelAsync();
        }

        [Test]
        public async Task SkippingInitialRun_RunsAfterInitialWait()
        {
            // Arrange
            var loggerMock = Substitute.For<ILogger<SkippingService>>();
            var classUnderTest = new SkippingService(loggerMock, BuildScopedContainer(), "*/1 * * * * *", skipInitial: true);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);

            // Assert
            await classUnderTest.HasStarted;
            await cancellationTokenSource.CancelAsync();
        }
    }
}
