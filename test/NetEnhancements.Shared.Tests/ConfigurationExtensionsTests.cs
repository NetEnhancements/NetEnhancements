using Microsoft.Extensions.Configuration;
using NetEnhancements.Shared.Configuration;
using ConfigurationExtensions = NetEnhancements.Shared.Configuration.ConfigurationExtensions;

namespace NetEnhancements.Shared.Tests
{
    public class ConfigurationExtensionsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetSectionName_Removes_Settings()
        {
            // Arrange & Act
            var sectionName = ConfigurationExtensions.GetSectionName<FakeSettings>();

            // Assert
            Assert.That(sectionName, Is.EqualTo("Fake"));
        }

        [Test]
        public void GetSectionOrThrow_Reads_Configuration()
        {
            // Arrange
            var myConfiguration = new Dictionary<string, string?>
            {
                {"Fake:Foo", "Foo"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            // Act
            var (section, settings) = configuration.GetSectionOrThrow<FakeSettings>();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(section, Is.Not.Null);
                Assert.That(settings, Is.Not.Null);

                Assert.That(settings.Foo, Is.EqualTo("Foo"));
            });
        }

        [Test]
        public void GetSectionOrThrow_Validates_Configuration()
        {
            // Arrange
            var myConfiguration = new Dictionary<string, string?>
            {
                // Instead of the required property "Foo", initialize "Bar".
                {"Fake:Bar", "Bar"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            // Act & Assert
            var exception = Assert.Throws<ConfigurationException>(() => configuration.GetSectionOrThrow<FakeSettings>());

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.Message, Contains.Substring("Foo"));
        }
    }
}