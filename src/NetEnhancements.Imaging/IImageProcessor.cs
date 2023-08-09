namespace NetEnhancements.Imaging;

/// <summary>
/// Contains methods for image manipulation.
/// </summary>
public interface IImageProcessor
{
    /// <summary>
    /// Resizes an image.
    /// </summary>
    /// <param name="imageStream">Source image.</param>
    /// <param name="resolution">Target resolution.</param>
    /// <param name="imageFormat">The format to save the resized image in. When default (<c>null</c>), takes the format of the input.</param>
    /// <param name="quality">The quality factor for the image. Defaults to 95.</param>
    /// <returns>A stream to the resized image.</returns>
    Task<Stream> ResizeAsync(Stream imageStream, Resolution resolution, ImageFormat? imageFormat = null, int quality = 95);
}