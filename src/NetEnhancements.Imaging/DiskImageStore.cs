namespace NetEnhancements.Imaging;

/// <summary>
/// Saves images to disk.
/// </summary>
internal class DiskImageStore : IImageStore
{
    private const string TrashDirectory = "_Trash";

    public async Task SaveImageAsync(Stream imageStream, string locationIdentifier, string imageIdentifier, string extension)
    {
        // Pre-emptively rewind.
        imageStream.Position = 0;

        // Use a predefined directory structure to prevent too many files in one directory.
        var filePath = GetDirectory(locationIdentifier, imageIdentifier);

        Directory.CreateDirectory(filePath);

        var diskFileName = Path.Combine(filePath, GetFileName(imageIdentifier, extension, resolution: null));

        await using var fileStream = File.OpenWrite(diskFileName);
        
        await imageStream.CopyToAsync(fileStream);
    }

    public Task DeleteAsync(string locationIdentifier, string imageIdentifier, string extension, ICollection<Resolution>? sizesToRemove = null, bool moveToTrash = true)
    {
        var filePath = GetDirectory(locationIdentifier, imageIdentifier);

        // Hard delete all resized images, if specified and if they exist.
        if (sizesToRemove != null)
        {
            foreach (var resolution in sizesToRemove)
            {
                var path = Path.Combine(
                    filePath,
                    GetFileName(imageIdentifier, extension, resolution)
                );

                if (!File.Exists(path))
                {
                    continue;
                }

                File.Delete(path);
            }
        }

        var fileName = GetFileName(imageIdentifier, extension, null);

        var originalPath = Path.Combine(
            filePath,
            fileName
        );

        // Optionally soft delete (move to _Trash) the original.
        if (moveToTrash)
        {
            var trashDirectory = Path.Combine(locationIdentifier, TrashDirectory);
            Directory.CreateDirectory(trashDirectory);

            File.Move(originalPath, Path.Combine(trashDirectory, fileName));
        }
        else
        {
            File.Delete(originalPath);
        }

        return Task.CompletedTask;
    }

    public Task<Stream> OpenStreamAsync(string basePath, string imageIdentifier, string extension, Resolution? resolution)
    {
        var path = Path.Combine(
            GetDirectory(basePath, imageIdentifier),
            GetFileName(imageIdentifier, extension, resolution)
        );

        return Task.FromResult((Stream)File.OpenRead(path));
    }

    /// <summary>
    /// 26*26 directories ought to be enough for anybody.
    /// </summary>
    private static string GetDirectory(string basePath, string id) => Path.Combine(new[]
    {
        basePath,
        // First letter of the GUID
        id[..1],
        // Second letter of the GUID
        id[1..2]
    });

    /// <summary>
    /// This assumes the imageType equals the file extension. True for jpeg/png/webp.
    /// </summary>
    private static string GetFileName(string identifier, string extension, Resolution? resolution)
    {
        string? infix = null;

        if (resolution != null)
        {
            infix = "_" + resolution.Width + "x" + resolution.Height;
        }

        return identifier.ToLower() + infix + "." + extension.ToLower();
    }
}
