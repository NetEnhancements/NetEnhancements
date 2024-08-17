namespace NetEnhancements.AspNet.StaticFiles
{
    /// <summary>
    /// Cacheable property container, so we don't have to hit the filesystem for every request.
    /// </summary>
    /// <param name="Exists">Whether the file exists on the filesystem.</param>
    /// <param name="Name">Filename.</param>
    /// <param name="PhysicalPath">Full path on filesystem.</param>
    /// <param name="SubPath">Path relative to wwwroot.</param>
    /// <param name="Length">File size in bytes.</param>
    /// <param name="LastModified">When the file was last written.</param>
    /// <param name="DataCacheable">Whether the file can be cached (i.e. exists and data less than file limit).</param>
    internal record PhysicalFileInfo
    (
        bool Exists,
        string Name,
        string PhysicalPath,
        string SubPath,
        DateTimeOffset LastModified,
        long Length,
        bool DataCacheable)
    {
        public static PhysicalFileInfo FromFileInfo(FileInfo fileInfo, string subPath, int maxFileSize) => new(
            fileInfo.Exists,
            fileInfo.Name,
            fileInfo.FullName,
            subPath,
            fileInfo.LastWriteTimeUtc,
            fileInfo.Exists ? fileInfo.Length : 0,
            fileInfo.Exists && fileInfo.Length <= maxFileSize
        );
    }
}
