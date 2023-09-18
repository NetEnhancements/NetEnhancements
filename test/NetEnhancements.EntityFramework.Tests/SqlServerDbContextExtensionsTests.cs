using NetEnhancements.EntityFramework.Tests.Relational;

namespace NetEnhancements.EntityFramework.Tests
{
    public class SqlServerDbContextExtensionsTests
    {
        [Test]
        public void EnableIdentityInsertAsync()
        {
            // Arrange
            var testContext = new InMemoryTestDbContext();

            // Act & Assert
            // We can't actually test what gets sent to the database without a whole lot of mocking. This is here for coverage.
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await testContext.EnableIdentityInsertAsync<TestEntity>(enable: true),
                "Relational-specific methods can only be used when the context is using a relational database provider."
            );
        }
    }
}
