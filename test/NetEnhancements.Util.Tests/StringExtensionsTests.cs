namespace NetEnhancements.Util.Tests
{
    public class StringExtensionsTests
    {
        [Test]
        public void RemoveEnd_Checks_Null()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _ = ((string?)null!).RemoveEnd("foo"));
            Assert.Throws<ArgumentNullException>(() => _ = "foo".RemoveEnd(null!));
        }
        
        [Test]
        [TestCase("Befoo", "foo", "Be")]
        [TestCase("foo", "foo", "")]
        [TestCase("fo", "foo", "fo")]
        [TestCase("foo", "o", "fo")]
        [TestCase("", "o", "")]
        public void RemoveEnd_Returns(string input, string suffix, string result)
        {
            Assert.That(input.RemoveEnd(suffix), Is.EqualTo(result));
        }
    }
}
