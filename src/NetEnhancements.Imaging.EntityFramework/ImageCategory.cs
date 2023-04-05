namespace NetEnhancements.Imaging.EntityFramework;

public class ImageCategory : ImageCategoryBase { }

/// <summary>
/// Junction table: relation between images and categories.
/// </summary>
public abstract class ImageCategoryBase
{
    public Guid ImageId { get; set; }

    public int CategoryId { get; set; }
}
