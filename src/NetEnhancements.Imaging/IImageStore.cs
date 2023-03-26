namespace NetEnhancements.Imaging;

/// <summary>
/// Image storage abstraction layer.
/// </summary>
public interface IImageStore
{
    /// <summary>
    /// Opens a stream to the source image. For the original, don't pass a <paramref name="resolution"/>.
    /// </summary>
    Task<Stream> OpenStreamAsync(string locationIdentifier, string imageIdentifier, string extension, Resolution? resolution);

    /// <summary>
    /// Saves the original version of an image.
    /// </summary>
    Task SaveOriginalAsync(Stream imageStream, string locationIdentifier, string imageIdentifier, string extension);
    
    /// <summary>
    /// Saves resized version of an original image.
    /// </summary>
    Task SaveResizedAsync(Stream imageStream, string locationIdentifier, string imageIdentifier, string extension, Resolution resolution);

    /// <summary>
    /// Deletes an original image, and the specified resized ones (<paramref name="sizesToRemove"/>). When <paramref name="moveToTrash"/> is <c>true</c>, the original will be moved to a location from where it can be (manually) restored.
    /// </summary>
    Task DeleteAsync(string locationIdentifier, string imageIdentifier, string extension, ICollection<Resolution>? sizesToRemove = null, bool moveToTrash = true);
}
