using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NetEnhancements.EntityFramework;

namespace NetEnhancements.Imaging.EntityFramework;

/// <summary>
/// Default class for storing image metadata.
/// </summary>
public class Image : ImageBase<Category> { }

/// <summary>
/// Base class for storing image metadata.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public abstract class ImageBase<TCategory> : GuidIdEntity
    where TCategory : CategoryBase
{
    /// <summary>
    /// The unique system name for this image.
    /// </summary>
    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string? Name { get; set; }

    /// <summary>
    /// The width in pixels.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height in pixels.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// A developer-specified string indicating the file type, e.g. "png"", "jpeg", ...
    /// </summary>
    [Required]
    [StringLength(16, MinimumLength = 1)]
    public string? ContentType { get; set; }

    /// <summary>
    /// The size of the image in storage bytes.
    /// </summary>
    [Required]
    public long FileSize { get; set; }

    /// <summary>
    /// The categories this image belongs to.
    /// </summary>
    public virtual ICollection<TCategory>? Categories { get; set; }
}