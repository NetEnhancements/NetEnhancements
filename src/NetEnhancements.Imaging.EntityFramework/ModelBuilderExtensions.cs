using Microsoft.EntityFrameworkCore;

namespace NetEnhancements.Imaging.EntityFramework;

public static class ModelBuilderExtensions
{
    public const string DefaultImageTableName = "Images";
    // "ImageCategories" for not colliding with the generic term "Categories".
    public const string DefaultCategoriesTableName = "ImageCategories"; 
    public const string DefaultImageCategoriesTableName = "ImageCategoryImages";

    /// <summary>
    /// Adds the Imaging data store using the specified <typeparamref name="TImage" />, <typeparamref name="TCategory"/> and <typeparamref name="TImageCategory"/> and default or specified table names.
    /// </summary>
    public static ModelBuilder UseImaging<TImage, TCategory, TImageCategory>(this ModelBuilder modelBuilder, string? imageTableName = null, string? categoryTableName = null, string? imageCategoryTableName = null)
        where TImage : ImageBase<TCategory>
        where TCategory : CategoryBase
        where TImageCategory : ImageCategoryBase
    {
        modelBuilder.Ignore<ImageBase<CategoryBase>>();
        modelBuilder.Ignore<CategoryBase>();
        modelBuilder.Ignore<ImageCategoryBase>();

        modelBuilder.Entity<TCategory>().ToTable(categoryTableName ?? DefaultCategoriesTableName);

        modelBuilder.Entity<TImage>()
            .ToTable(imageTableName ?? DefaultImageTableName)
            .HasMany(p => p.Categories)
            .WithMany()
            .UsingEntity<TImageCategory>(
                join => join.HasOne<TCategory>().WithMany().HasForeignKey(ic => ic.CategoryId),
                join => join.HasOne<TImage>().WithMany().HasForeignKey(ic => ic.ImageId),
                join => join.ToTable(imageCategoryTableName ?? DefaultImageCategoriesTableName)
            );

        return modelBuilder;
    }

    /// <summary>
    /// Adds the Imaging data store using the default entities and default or specified table names.
    /// </summary>
    public static ModelBuilder UseImaging(this ModelBuilder modelBuilder, string? imageTableName = null, string? categoryTableName = null, string? imageCategoryTableName = null)
        => modelBuilder.UseImaging<Image, Category, ImageCategory>(imageTableName, categoryTableName, imageCategoryTableName);

    /// <summary>
    /// Adds the Imaging data store using the specified <typeparam name="TImage">image type</typeparam> and otherwise default entities and default or specified table names.
    /// </summary>
    public static ModelBuilder UseImaging<TImage>(this ModelBuilder modelBuilder, string? imageTableName = null, string? categoryTableName = null, string? imageCategoryTableName = null)
        where TImage : ImageBase<Category>
        => modelBuilder.UseImaging<TImage, Category, ImageCategory>(imageTableName, categoryTableName, imageCategoryTableName);

    /// <summary>
    /// Adds the Imaging data store using the specified <typeparamref name="TImage" /> and <typeparamref name="TCategory"/> and otherwise default entities and default or specified table names.
    /// </summary>
    public static ModelBuilder UseImaging<TImage, TCategory>(this ModelBuilder modelBuilder, string? imageTableName = null, string? categoryTableName = null, string? imageCategoryTableName = null)
        where TImage : ImageBase<TCategory>
        where TCategory : CategoryBase
        => modelBuilder.UseImaging<TImage, TCategory, ImageCategory>(imageTableName, categoryTableName, imageCategoryTableName);
}
