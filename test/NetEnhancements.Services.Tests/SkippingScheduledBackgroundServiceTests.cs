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
        public void InvalidSchedule_Throws()
        {
            // Arrange
            // Invalid because seconds are mandatory.
            const string invalidPattern = "* * * * *";

            var loggerMock = Substitute.For<ILogger<SkippingService>>();
            var classUnderTest = new SkippingService(loggerMock, BuildScopedContainer(), invalidPattern, skipInitial: false);
            var cancellationTokenSource = new CancellationTokenSource();

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                // Act
                await classUnderTest.StartAsync(cancellationTokenSource.Token);
            });
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
            await cancellationTokenSource.CancelAsync();
            Assert.That(classUnderTest.IsCalled, Is.True);
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
            await cancellationTokenSource.CancelAsync();
            Assert.That(classUnderTest.IsCalled, Is.False);
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
            await Task.Delay(TimeSpan.FromMilliseconds(1200), cancellationTokenSource.Token);

            // Assert
            Assert.That(classUnderTest.IsCalled, Is.True);
        }
    }
}
