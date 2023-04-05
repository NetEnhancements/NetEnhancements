using SkiaSharp;

namespace NetEnhancements.Imaging
{
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

            var info = new ImageInfo(
                skiaCodec.EncodedFormat.ToString().ToLower(), 
                skiaCodec.Info.Width, 
                skiaCodec.Info.Height
            );

            return Task.FromResult<ImageInfo?>(info);
        }
    }
}