namespace NetEnhancements.Util.Tests
{
    public class CollectionExtensionTests
    {
        [Test]
        public void In_IEnumerable()
        {
            // Arrange
            var enumList = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };

            // Act
            var isIn = DayOfWeek.Friday.In(enumList);

            Assert.That(isIn, Is.True);
        }

        [Test]
        public void In_Array()
        {
            // Arrange & Act
            var isIn = DayOfWeek.Friday.In(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday);

            Assert.That(isIn, Is.True);
        }
    }
}
