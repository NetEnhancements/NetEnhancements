using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using NetEnhancements.Shared.EntityFramework;

namespace NetEnhancements.Imaging.EntityFramework;

public class Category : CategoryBase { }

[Index(nameof(Name), IsUnique = true)]
public abstract class CategoryBase : IntIdEntity
{
    [Required]
    [StringLength(16, MinimumLength = 1)]
    public string? Name { get; set; }
}
