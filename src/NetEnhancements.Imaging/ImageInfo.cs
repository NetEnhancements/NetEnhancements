namespace NetEnhancements.Imaging;

/// <summary>
/// Image characteristics
/// </summary>
public record ImageInfo(ImageFormat Format, int Width, int Height, long FileSize) : Resolution(Width, Height);

/// <summary>
/// Width and height of media.
/// </summary>
public record Resolution(int Width, int Height);
