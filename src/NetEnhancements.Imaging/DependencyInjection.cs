using Microsoft.Extensions.DependencyInjection;

namespace NetEnhancements.Imaging;

/// <summary>
/// Imaging-specific Dependency Injection Builder.
/// </summary>
public class ImagingBuilder
{
    /// <summary>
    /// The service collection to add additional services to.
    /// </summary>
    public IServiceCollection Services { get; }

    internal ImagingBuilder(IServiceCollection services)
    {
        Services = services;
    }
}

/// <summary>
/// Dependency Injection container extensions.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Register the services necessary for image inspection and manipulation.
    /// </summary>
    public static ImagingBuilder AddImaging(this IServiceCollection services)
    {
        var defaultProcessor = new SkiaImageProcessor();

        services.AddSingleton<IImageInspector>(defaultProcessor);
        services.AddSingleton<IImageProcessor>(defaultProcessor);

        return new ImagingBuilder(services);
    }

    /// <summary>
    /// Add disk storage to an <see cref="ImagingBuilder"/>.
    /// </summary>
    public static ImagingBuilder AddDiskStore(this ImagingBuilder builder)
    {
        builder.Services.AddScoped<IImageStore, DiskImageStore>();

        return builder;
    }
}
