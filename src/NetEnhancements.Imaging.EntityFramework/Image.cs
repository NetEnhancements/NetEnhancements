using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using NetEnhancements.EntityFramework;

namespace NetEnhancements.Imaging.EntityFramework;

public class Image : ImageBase<Category> { }

/// <summary>
/// Images can be used as PDF page backgrounds. The pages they can belong to are stored in the <see cref="PdfPage"/>s table, entries there are seeded.
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