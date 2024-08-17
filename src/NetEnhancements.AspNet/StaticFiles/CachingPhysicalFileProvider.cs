using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace NetEnhancements.AspNet.StaticFiles
{
    /// <summary>
    /// Serves files from the filesystem, after which they are cached in-memory for faster responses.
    ///
    /// The design of this class assumes that files under the root are never changed during the application lifetime.
    /// </summary>
    public class CachingPhysicalFileProvider : IFileProvider
    {
        private readonly string _root;
        private readonly IMemoryCache _memoryCache;
        private readonly int _maxFileSize;
        private readonly TimeSpan _slidingExpiration;

        /// <summary>
        /// Initializes a new instance of a PhysicalFileProvider at the wwwroot directory.
        /// </summary>
        public CachingPhysicalFileProvider(IWebHostEnvironment hostEnvironment) : this(hostEnvironment, new PhysicalFileCachingOptions()) { }

        /// <summary>
        /// Initializes a new instance of a PhysicalFileProvider at the wwwroot directory with the given options.
        /// </summary>
        /// <param name="hostEnvironment"></param>
        /// <param name="options"></param>
        public CachingPhysicalFileProvider(IWebHostEnvironment hostEnvironment, PhysicalFileCachingOptions options)
        {
            if (options.MaxSingleFileSize > options.MaxCacheSize)
            {
                throw new InvalidOperationException($"Configure a {nameof(options.MaxSingleFileSize)} ({options.MaxSingleFileSize}) that's less than the {nameof(options.MaxCacheSize)} ({options.MaxCacheSize}).");
            }

            if (string.IsNullOrWhiteSpace(options.Root))
            {
                options.Root = Path.GetFullPath(Path.Combine(hostEnvironment.ContentRootPath, string.IsNullOrWhiteSpace(hostEnvironment.WebRootPath) ? "wwwroot" : hostEnvironment.WebRootPath));
            }

            if (!Path.IsPathRooted(options.Root))
            {
                throw new ArgumentException("The path must be absolute.", nameof(options.Root));
            }

            string fullRoot = Path.GetFullPath(options.Root);

            // When we do matches in GetFullPath, we want to only match full directory names.
            _root = PathUtils.EnsureTrailingSlash(fullRoot);

            if (!Directory.Exists(_root))
            {
                throw new DirectoryNotFoundException(_root);
            }

            _memoryCache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = options.MaxCacheSize
            });

            _maxFileSize = options.MaxSingleFileSize;

            if (options.Expiration < TimeSpan.FromSeconds(1))
            {
                options.Expiration = new PhysicalFileCachingOptions().Expiration;
            }
            
            _slidingExpiration = options.Expiration;
        }

        private string? GetFullPath(string path)
        {
            if (PathUtils.PathNavigatesAboveRoot(path))
            {
                return null;
            }

            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(Path.Combine(_root, path));
            }
            catch
            {
                return null;
            }

            if (!IsUnderneathRoot(fullPath))
            {
                return null;
            }

            return fullPath;
        }

        private bool IsUnderneathRoot(string fullPath) => fullPath.StartsWith(_root, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Locate a file at the given path by directly mapping path segments to physical directories.
        /// </summary>
        /// <param name="subpath">A path under the root directory</param>
        /// <returns>The file information. Caller must check <see cref="IFileInfo.Exists"/> property. </returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath) || PathUtils.HasInvalidPathChars(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            // Relative paths starting with leading slashes are okay
            subpath = subpath.TrimStart(PathUtils.PathSeparators);

            // Absolute paths not permitted.
            if (Path.IsPathRooted(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var cacheKey = $"phys-file-info-{subpath}";

            if (!_memoryCache.TryGetValue(cacheKey, out PhysicalFileInfo? cachedFileInfo) || cachedFileInfo == null)
            {
                string? fullPath = GetFullPath(subpath);

                if (fullPath == null)
                {
                    return new NotFoundFileInfo(subpath);
                }

                var fileInfo = new FileInfo(fullPath);

                if (FileSystemInfoHelper.IsExcluded(fileInfo, ExclusionFilters.Sensitive))
                {
                    return new NotFoundFileInfo(subpath);
                }

                cachedFileInfo = PhysicalFileInfo.FromFileInfo(fileInfo, subpath, _maxFileSize);

                _memoryCache.Set(cacheKey, cachedFileInfo, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = _slidingExpiration,
                    Size = cachedFileInfo.Length,
                });
            }

            return new CachedPhysicalFileInfo(cachedFileInfo, _memoryCache, _slidingExpiration);
        }

        /// <inheritdoc/>
        public IDirectoryContents GetDirectoryContents(string subpath) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IChangeToken Watch(string filter) => throw new NotImplementedException();
    }
}
