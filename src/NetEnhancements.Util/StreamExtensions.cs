namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for <see cref="Stream"/>s.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Read a stream entirely, optionally not disposing it.
        /// </summary>
        public static byte[] ReadFully(this Stream stream, bool dispose = true)
        {
            using MemoryStream memoryStream = new();

            stream.CopyTo(memoryStream);
            
            if (dispose)
            { 
                // We're done with the source stream, and so should the caller.
                stream.Dispose();
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Read a stream entirely, optionally not disposing it.
        /// </summary>
        public static async Task<byte[]> ReadFullyAsync(this Stream stream, bool dispose = true, CancellationToken cancellationToken = default)
        {
            using MemoryStream memoryStream = new();

            await stream.CopyToAsync(memoryStream, cancellationToken);

            if (dispose)
            {
                // We're done with the source stream, and so should the caller.
                await stream.DisposeAsync();
            }

            return memoryStream.ToArray();
        }
    }
}
