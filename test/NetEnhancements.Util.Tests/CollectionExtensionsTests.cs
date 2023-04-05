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

        [Test]
        public void GetPermutations_Without_Length_Returns_All_Permutations()
        {
            // Arrange
            List<int> inputList = new List<int> { 1, 2, 3 };
            IEnumerable<List<int>> expectedPermutations = new List<List<int>>
                                                       {
                                                           new() { 1, 2, 3 },
                                                           new() { 1, 3, 2 },
                                                           new() { 2, 1, 3 },
                                                           new() { 2, 3, 1 },
                                                           new() { 3, 1, 2 },
                                                           new() { 3, 2, 1 }
                                                       };

            // Act
            IEnumerable<IEnumerable<int>> result = inputList.GetPermutations();

            // Assert
            CollectionAssert.AreEquivalent(expectedPermutations, result.Select(p => p.ToList()));
        }

        [Test]
        public void GetPermutations_Groups_By_Length()
        {
            // Arrange
            List<int> inputList = new List<int> { 1, 2, 3 };
            int length = 2;
            List<List<int>> expectedPermutations = new List<List<int>>
                                                       {
                                                           new() { 1, 2 },
                                                           new() { 1, 3 },
                                                           new() { 2, 1 },
                                                           new() { 2, 3 },
                                                           new() { 3, 1 },
                                                           new() { 3, 2 }
                                                       };

            // Act
            IEnumerable<IEnumerable<int>> result = inputList.GetPermutations(length);

            // Assert
            CollectionAssert.AreEquivalent(expectedPermutations, result.Select(p => p.ToList()));
        }

        [Test]
        public void GetPermutations_Throws_When_Lenght_Is_Larger_Than_Collection_Count()
        {
            // Arrange
            List<int> inputList = new List<int> { 1, 2, 3 };
            int length = 4;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = inputList.GetPermutations(length).ToList());
        }
    }
}
