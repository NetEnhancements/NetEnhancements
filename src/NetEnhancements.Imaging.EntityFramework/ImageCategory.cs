namespace NetEnhancements.Imaging.EntityFramework;

/// <summary>
/// Default image category junction table entity.
/// </summary>
public class ImageCategory : ImageCategoryBase { }

/// <summary>
/// Junction table: relation between images and categories.
/// </summary>
public abstract class ImageCategoryBase
{
    /// <summary>
    /// The ID of the image.
    /// </summary>
    public Guid ImageId { get; set; }

    /// <summary>
    /// The ID of the category the image is in.
    /// </summary>
    public int CategoryId { get; set; }
}
