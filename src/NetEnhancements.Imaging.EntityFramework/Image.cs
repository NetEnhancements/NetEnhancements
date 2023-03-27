using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string? Name { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 1)]
    public string? ContentType { get; set; }

    public virtual ICollection<TCategory>? Categories { get; set; }
}