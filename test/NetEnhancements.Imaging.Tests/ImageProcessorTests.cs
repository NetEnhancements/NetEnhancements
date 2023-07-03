namespace NetEnhancements.Imaging.Tests
{
    public class ImageProcessorTests
    {
        [Test]
        public async Task GetImageInfoAsync_Gets_ImageInfo()
        {
            // Arrange
            var imageProcessor = new SkiaImageProcessor();
            var imageStream = File.OpenRead("TestImage.png");

            // Act
            var imageInfo = await imageProcessor.GetImageInfoAsync(imageStream);

            // Assert
            Assert.That(imageInfo, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(imageInfo!.Format, Is.EqualTo("png"));
                Assert.That(imageInfo.Width, Is.EqualTo(42));
                Assert.That(imageInfo.Height, Is.EqualTo(21));
                Assert.That(imageInfo.FileSize, Is.EqualTo(643));
            });
        }
    }
}