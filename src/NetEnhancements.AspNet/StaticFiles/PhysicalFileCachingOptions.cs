namespace NetEnhancements.AspNet.StaticFiles
{
    /// <summary>
    /// Configures the cache options.
    /// </summary>
    public class PhysicalFileCachingOptions
    {
        /// <summary>
        /// The root directory.This must be an absolute path. When not provided, defaults to the <see cref="Microsoft.AspNetCore.Hosting.IWebHostEnvironment.WebRootPath"/> (default: wwwroot) under the application directory, taken from <see cref="Microsoft.Extensions.Hosting.IHostEnvironment.ContentRootPath"/>.
        /// </summary>
        public string? Root { get; set; }

        /// <summary>
        /// The cache size. Defaults to 16 MiB.
        /// </summary>
        public int MaxCacheSize { get; set; } = 16 * 1024 * 1024;

        /// <summary>
        /// The maximum size of a single file to cache. Defaults to 1 MiB.
        /// </summary>
        public int MaxSingleFileSize { get; set; } = 1 * 1024 * 1024;

        /// <summary>
        /// The sliding expiration time after which a cache entry will be evicted if it's not accessed.
        /// Defaults to one hour when not specified or configured to be less than one second.
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(1);
    }
}
