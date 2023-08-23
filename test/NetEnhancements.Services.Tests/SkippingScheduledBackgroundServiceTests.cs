using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NetEnhancements.Services.Tests.Services;

namespace NetEnhancements.Services.Tests
{
    public class SkippingScheduledBackgroundServiceTests
    {
        /// <summary>
        /// TODO: doesn't any other package offer this?
        /// </summary>
        /// <returns></returns>
        private static IServiceContainer BuildScopedContainer()
        {
            var containerFake = new ServiceContainer();

            var scopeMock = new Mock<IServiceScope>();
            scopeMock.SetupGet(s => s.ServiceProvider).Returns(containerFake);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
            scopeFactoryMock.Setup(s => s.CreateScope()).Returns(scopeMock.Object);

            containerFake.AddService(typeof(IServiceScopeFactory), scopeFactoryMock.Object);

            return containerFake;
        }

        [Test]
        public void InvalidSchedule_Throws()
        {
            // Arrange
            // Invalid because seconds are mandatory.
            const string invalidPattern = "* * * * *";

            var loggerMock = new Mock<ILogger<SkippingService>>(MockBehavior.Loose);
            var classUnderTest = new SkippingService(loggerMock.Object, BuildScopedContainer(), invalidPattern, skipInitial: false);
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
            var loggerMock = new Mock<ILogger<SkippingService>>(MockBehavior.Loose);
            var classUnderTest = new SkippingService(loggerMock.Object, BuildScopedContainer(), "*/1 * * * * *", skipInitial: false);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);

            // Assert
            cancellationTokenSource.Cancel();
            Assert.That(classUnderTest.IsCalled, Is.True);
        }

        [Test]
        public async Task SkippingInitialRun_WaitsImmediately()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SkippingService>>(MockBehavior.Loose);
            var classUnderTest = new SkippingService(loggerMock.Object, BuildScopedContainer(), "*/1 * * * * *", skipInitial: true);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);

            // Assert
            cancellationTokenSource.Cancel();
            Assert.That(classUnderTest.IsCalled, Is.False);
        }

        [Test]
        public async Task SkippingInitialRun_RunsAfterInitialWait()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SkippingService>>(MockBehavior.Loose);
            var classUnderTest = new SkippingService(loggerMock.Object, BuildScopedContainer(), "*/1 * * * * *", skipInitial: true);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await classUnderTest.StartAsync(cancellationTokenSource.Token);
            await Task.Delay(TimeSpan.FromMilliseconds(1200), cancellationTokenSource.Token);

            // Assert
            Assert.That(classUnderTest.IsCalled, Is.True);
        }
    }
}