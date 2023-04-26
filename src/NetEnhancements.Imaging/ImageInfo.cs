namespace NetEnhancements.Imaging;

/// <summary>
/// Image characteristics
/// </summary>
public record ImageInfo(string Format, int Width, int Height) : Resolution(Width, Height);

/// <summary>
/// Width and height of media.
/// </summary>
public record Resolution(int Width, int Height);
