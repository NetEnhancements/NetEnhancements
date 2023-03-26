using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NetEnhancements.Shared.Configuration
{
    /// <summary>
    /// Extension method container for settings.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Reads, registers (optionally and by default as <see cref="IOptions{TSettings}"/>) and returns a <typeparamref name="TSettings"/> instance, read from <paramref name="configuration"/>.
        ///
        /// Throws if the section is missing or if it contains invalid data according to its data annotations.
        /// </summary>
        public static TSettings RegisterSettings<TSettings>(this IServiceCollection services, IConfiguration configuration, string? sectionName = null, bool registerIOptions = true)
            where TSettings : class
        {
            var (section, settings) = configuration.GetSectionOrThrow<TSettings>(sectionName);

            services.Configure<TSettings>(section);
            
            if (registerIOptions)
            {
                services.AddOptions<TSettings>()
                        .ValidateDataAnnotations();
            }

            return settings;
        }
    }
}
