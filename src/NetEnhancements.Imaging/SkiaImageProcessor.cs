using SkiaSharp;

namespace NetEnhancements.Imaging;

internal class SkiaImageProcessor : IImageInspector, IImageProcessor
{
    public Task<ImageInfo?> GetImageInfoAsync(Stream imageData)
    {
        using var skiaStream = new SKManagedStream(imageData);

        using var skiaCodec = SKCodec.Create(skiaStream) ?? throw new InvalidOperationException("Could not decode image stream");

        var length = imageData.Length;

        var info = new ImageInfo(
            Map(skiaCodec.EncodedFormat),
            skiaCodec.Info.Width,
            skiaCodec.Info.Height,
            length
        );

        return Task.FromResult<ImageInfo?>(info);
    }

    public Task<Stream> ResizeAsync(Stream imageStream, Resolution resolution, ImageFormat? imageFormat = null, int quality = 95)
    {
        var skiaStream = new SKManagedStream(imageStream, disposeManagedStream: false);

        using var skiaCodec = SKCodec.Create(skiaStream) ?? throw new InvalidOperationException("Could not decode image stream");

        using var bitmap = SKBitmap.Decode(skiaCodec);

        // Possibly not equal to desired size, because of ratios.
        var targetResolution = SizeCalculator.GetRelativeSize(new(bitmap.Width, bitmap.Height), resolution);

        var newSize = new SKSizeI(targetResolution.Width, targetResolution.Height);

        using var resizedBitmap = bitmap.Resize(newSize, SKFilterQuality.High);

        var resizedImageFormat = imageFormat != null ? Map(imageFormat.Value) : skiaCodec.EncodedFormat;

        Stream memoryStream = new MemoryStream();

        if (!resizedBitmap.Encode(memoryStream, resizedImageFormat, quality))
        {
            // TODO: log, but there's nothing we can do.
            throw new InvalidOperationException("Could not resize the image");
        }

        // Rewind to read from start while saving.
        memoryStream.Position = 0;

        return Task.FromResult(memoryStream);
    }

    private static SKEncodedImageFormat Map(ImageFormat imageFormat)
    {
        return imageFormat switch
        {
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Png => SKEncodedImageFormat.Png,
            ImageFormat.WebP => SKEncodedImageFormat.Webp,

            _ => throw new ArgumentException($"Unknown format {imageFormat}")
        };
    }

    private static ImageFormat Map(SKEncodedImageFormat imageFormat)
    {
        return imageFormat switch
        {
            SKEncodedImageFormat.Jpeg => ImageFormat.Jpeg,
            SKEncodedImageFormat.Png => ImageFormat.Png,
            SKEncodedImageFormat.Webp => ImageFormat.WebP,

            _ => throw new ArgumentException($"Unsupported format {imageFormat}")
        };
    }
}