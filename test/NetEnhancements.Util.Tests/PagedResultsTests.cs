namespace NetEnhancements.Util.Tests
{
    public class PagedResultsTests
    {
        private class TestEntity
        {
            public int Id => 42;
        }

        [Test]
        public void PagedResults_Parameterless_Ctor_Requires_All_Properties()
        {
            // Arrange
            const int currentPage = 1;
            const int itemsPerPage = 2;
            const int totalPages = 2;
            const int totalItems = 3;

            // Act - test parameterless constructor with required properties
            var classUnderTest = new PagedResults<TestEntity>
            {
                Items = [new TestEntity()],
                TotalItems = totalItems,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage
            };

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(classUnderTest.Items, Has.Count.EqualTo(1));
                Assert.That(classUnderTest.CurrentPage, Is.EqualTo(currentPage));
                Assert.That(classUnderTest.ItemsPerPage, Is.EqualTo(itemsPerPage));
                Assert.That(classUnderTest.TotalItems, Is.EqualTo(totalItems));
                Assert.That(classUnderTest.TotalPages, Is.EqualTo(totalPages));
            }
        }

        [Test]
        public void PagedResults_Parameterized_Ctor_Initializes_All_Properties()
        {
            // Arrange
            const int currentPage = 1;
            const int itemsPerPage = 2;
            const int totalPages = 2;
            const int totalItems = 3;

            // Act - test parameterized constructor
            var classUnderTest = new PagedResults<TestEntity>
            (
                items: [new TestEntity()],
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                totalItems: totalItems
            );

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(classUnderTest.Items, Has.Count.EqualTo(1));
                Assert.That(classUnderTest.CurrentPage, Is.EqualTo(currentPage));
                Assert.That(classUnderTest.ItemsPerPage, Is.EqualTo(itemsPerPage));
                Assert.That(classUnderTest.TotalItems, Is.EqualTo(totalItems));
                Assert.That(classUnderTest.TotalPages, Is.EqualTo(totalPages));
            }
        }
    }
}
