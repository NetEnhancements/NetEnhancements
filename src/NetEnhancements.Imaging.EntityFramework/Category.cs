using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using NetEnhancements.EntityFramework;

namespace NetEnhancements.Imaging.EntityFramework;


/// <summary>
/// Default class for storing image categories.
/// </summary>
public class Category : CategoryBase { }

/// <summary>
/// Base class for storing image categories.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public abstract class CategoryBase : IntIdEntity
{
    /// <summary>
    /// The name of the category.
    /// </summary>
    [Required]
    [StringLength(16, MinimumLength = 1)]
    public string? Name { get; set; }
}
