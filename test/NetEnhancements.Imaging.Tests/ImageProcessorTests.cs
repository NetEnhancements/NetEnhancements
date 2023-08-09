namespace NetEnhancements.Imaging.Tests
{
    public class ImageProcessorTests
    {
        [Test]
        public async Task ResizeAsync_Uses_ImageFormat_And_Resolution()
        {
            // Arrange
            var imageProcessor = new SkiaImageProcessor();
            var imageStream = File.OpenRead("TestImage.png");

            // Act
            var resizedStream = await imageProcessor.ResizeAsync(imageStream, new Resolution(20, 19), ImageFormat.WebP);

            await using (var outStream = File.OpenWrite("a.webp"))
            {
                await resizedStream.CopyToAsync(outStream);
            }

            await using var inStream = File.OpenRead("a.webp");

            var resizedImageInfo = await imageProcessor.GetImageInfoAsync(inStream);

            // Assert
            Assert.That(resizedImageInfo, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(resizedImageInfo!.Format, Is.EqualTo(ImageFormat.WebP));
                Assert.That(resizedImageInfo.Width, Is.EqualTo(38));
                Assert.That(resizedImageInfo.Height, Is.EqualTo(19));
            });
        }
    }
}