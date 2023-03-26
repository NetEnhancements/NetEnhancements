using Microsoft.Extensions.DependencyInjection;

namespace NetEnhancements.Imaging;

public class ImagingBuilder
{
    public IServiceCollection Services { get; }

    internal ImagingBuilder(IServiceCollection services)
    {
        Services = services;
    }
}

public static class DependencyInjection
{
    public static ImagingBuilder AddImaging(this IServiceCollection services)
    {
        var defaultProcessor = new SkiaImageProcessor();

        services.AddSingleton<IImageInspector>(defaultProcessor);
        services.AddSingleton<IImageProcessor>(defaultProcessor);

        return new ImagingBuilder(services);
    }

    public static ImagingBuilder AddDiskStore(this ImagingBuilder builder)
    {
        builder.Services.AddScoped<IImageStore, DiskImageStore>();

        return builder;
    }
}
