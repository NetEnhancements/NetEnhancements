using SkiaSharp;

namespace NetEnhancements.Imaging;

internal class SkiaImageProcessor : IImageInspector, IImageProcessor
{
    public Task<ImageInfo?> GetImageInfoAsync(Stream imageData)
    {
        using var skiaStream = new SKManagedStream(imageData);

        using var skiaCodec = SKCodec.Create(skiaStream);

        if (skiaCodec == null)
        {
            return Task.FromResult((ImageInfo?)null);
        }

        var fileSize = skiaStream.Length;

        var info = new ImageInfo(
            skiaCodec.EncodedFormat.ToString().ToLower(), 
            skiaCodec.Info.Width, 
            skiaCodec.Info.Height,
            fileSize
        );

        return Task.FromResult<ImageInfo?>(info);
    }

    public Task<Stream> ResizeAsync(Stream imageStream, Resolution resolution)
    {
        using var bitmap = SKBitmap.Decode(new SKManagedStream(imageStream, disposeManagedStream: false));

        // Possibly not equal to desired size, because of ratios.
        var targetResolution = SizeCalculator.GetRelativeSize(new(bitmap.Width, bitmap.Height), resolution);

        using var resizedBitmap = bitmap.Resize(new SKSizeI(targetResolution.Width, targetResolution.Height), SKFilterQuality.High);

        Stream memoryStream = new MemoryStream();

        if (!resizedBitmap.Encode(memoryStream, SKEncodedImageFormat.Png, quality: 95))
        {
            // TODO: log, but there's nothing we can do.
            throw new InvalidOperationException("Could not resize the image");
        }

        // Rewind to read from start while saving.
        memoryStream.Position = 0;

        return Task.FromResult(memoryStream);
    }
}