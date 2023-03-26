namespace NetEnhancements.Imaging;

public record ImageInfo(string Format, int Width, int Height) : Resolution(Width, Height);

public record Resolution(int Width, int Height);
