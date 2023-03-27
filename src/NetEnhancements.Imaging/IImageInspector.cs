namespace NetEnhancements.Imaging;

/// <summary>
/// Retrieve image information.
/// </summary>
public interface IImageInspector
{
    /// <summary>
    /// Returns format and dimensions of the image contained within the stream, or null if it can't be determined.
    /// </summary>
    Task<ImageInfo?> GetImageInfoAsync(Stream imageData);
}
