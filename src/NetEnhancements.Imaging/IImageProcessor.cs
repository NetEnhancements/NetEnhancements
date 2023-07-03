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
    /// <returns>A stream to the resized image.</returns>
    Task<Stream> ResizeAsync(Stream imageStream, Resolution resolution);
}