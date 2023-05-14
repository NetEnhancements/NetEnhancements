namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for <see cref="Stream"/>s.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads an entire <see cref="Stream"/> and returns its contents as a byte array.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read.</param>
        /// <param name="dispose">Whether to dispose of the <paramref name="stream"/> after reading it.</param>
        /// <returns>The contents of the <paramref name="stream"/> as a byte array.</returns>
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
        /// Reads an entire <see cref="Stream"/> asynchronously and returns its contents as a byte array.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read.</param>
        /// <param name="dispose">Whether to dispose of the <paramref name="stream"/> after reading it.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the operation to complete.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result of the task contains the contents of the <paramref name="stream"/> as a byte array.</returns>
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
