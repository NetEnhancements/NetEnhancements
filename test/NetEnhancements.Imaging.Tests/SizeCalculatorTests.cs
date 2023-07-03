namespace NetEnhancements.Imaging.Tests
{
    [TestFixture]
    public class SizeCalculatorTests
    {
        [Test]
        [TestCaseSource(nameof(GetRelativeSize_DataSource))]
        public void GetRelativeSize_Resizes((Resolution Original, Resolution Desired, Resolution Expected) testData)
        {
            // Arrange, Act
            var newSize = SizeCalculator.GetRelativeSize(testData.Original, testData.Desired);

            // Assert
            Assert.That(newSize, Is.EqualTo(testData.Expected));
        }

        private static List<(Resolution Original, Resolution Desired, Resolution Expected)> GetRelativeSize_DataSource()
        {
            return new List<(Resolution Original, Resolution Desired, Resolution Expected)>
            {
                (new (200, 100), new (100, 50), new (100, 50)),
                (new (200, 100), new (100, 48), new (96, 48)),
                (new (100, 200), new (100, 48), new (24, 48)),
                (new (100, 200), new (100, 50), new (25, 50)),
                (new (100, 100), new (50, 50), new (50, 50)),
                (new (10, 20), new (8, 20), new (8, 16)),
                (new (42, 42), new (21, 21), new (21, 21)),
                (new (42, 42), new (21, 8), new (8, 8)),
                (new (42, 42), new (50, 50), new (50, 50)),
                (new (42, 42), new (50, 60), new (50, 50)),
                (new (42, 42), new (21, 16), new (16, 16)),
            };
        }
    }
}