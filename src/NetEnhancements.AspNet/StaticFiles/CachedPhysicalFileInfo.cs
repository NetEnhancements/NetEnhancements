using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;

namespace NetEnhancements.AspNet.StaticFiles;

/// <summary>
/// Container class that gets instantiated for each response.
/// </summary>
internal class CachedPhysicalFileInfo(PhysicalFileInfo fileInfo, IMemoryCache memoryCache, TimeSpan slidingExpiration) : IFileInfo
{
    private readonly string _subPath = fileInfo.SubPath;

    /// <summary>
    /// Do not store this in the <see cref="PhysicalPath"/> property, or ASP.NET will send the file by itself and not call <see cref="CreateReadStream"/>.
    /// </summary>
    private readonly string _physicalPath = fileInfo.PhysicalPath;

    /// <inheritdoc />
    public bool Exists => fileInfo.Exists;

    /// <inheritdoc />
    public long Length => fileInfo.Length;

    /// <summary>
    /// Always null, so <see cref="CreateReadStream"/> gets called.
    /// </summary>
    public string? PhysicalPath => null;

    /// <inheritdoc />
    public string Name => fileInfo.Name;

    /// <inheritdoc />
    public DateTimeOffset LastModified => fileInfo.LastModified;

    /// <summary>
    /// Always false.
    /// </summary>
    public bool IsDirectory => false;

    /// <summary>
    /// Reads the entire file into a byte array, saves that in the <see cref="IMemoryCache"/> and returns a <see cref="MemoryStream"/> pointing to that file buffer.
    /// </summary>
    public Stream CreateReadStream()
    {
        if (!fileInfo.DataCacheable)
        {
            // We are setting buffer size to 1 to prevent FileStream from allocating its internal buffer, 0 causes constructor to throw.
            // This lets the web server handle the buffering?
            const int bufferSize = 1;

            return new FileStream(
                _physicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                bufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
        }

        var cacheKey = $"phys-file-data-{_subPath}";

        // TODO: this actually reads the entire file while the caller just expects a stream back.
        // Implement a MemoryStream wrapper that caches on read?
        if (!memoryCache.TryGetValue(cacheKey, out byte[]? buffer) || buffer == null)
        {
            buffer = File.ReadAllBytes(_physicalPath);

            memoryCache.Set(cacheKey, buffer, new MemoryCacheEntryOptions
            {
                Size = buffer.Length,
                SlidingExpiration = slidingExpiration
            });
        }

        return new MemoryStream(buffer, writable: false, publiclyVisible: false, index: 0, count: buffer.Length);
    }
}