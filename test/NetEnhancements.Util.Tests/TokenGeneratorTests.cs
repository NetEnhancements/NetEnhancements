namespace NetEnhancements.Util.Tests
{
    public class RandomExtensionsTests
    {
        [Test]
        public void GeneratedStringLength_Equals_Parameter()
        {
            // Arrange
            const int tokenLength = 32;

            // Act
            var randomToken = RandomExtensions.GenerateRandomToken(tokenLength);

            // Assert
            Assert.That(randomToken, Has.Length.EqualTo(tokenLength));
        }

        [Test]
        public void ThrowsOnOdd()
        {
            // Arrange
            const int tokenLength = 1;

            // Act
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => _ = RandomExtensions.GenerateRandomToken(tokenLength));
        }
    }
}
