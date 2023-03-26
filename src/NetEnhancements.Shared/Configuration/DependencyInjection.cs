using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetEnhancements.Shared.Configuration
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers and returns a <typeparamref name="TSettings"/> object, read from <paramref name="configuration"/>. Throws if missing or invalid.
        /// </summary>
        public static TSettings RegisterSettings<TSettings>(this IServiceCollection services, IConfiguration configuration, string? sectionName = null, bool registerIOptions = true)
            where TSettings : class
        {
            var (section, settings) = configuration.GetSectionOrThrow<TSettings>(sectionName);

            if (registerIOptions)
            {
                services.Configure<TSettings>(section)
                        .AddOptions<TSettings>()
                        .ValidateDataAnnotations();
            }

            return settings;
        }
    }
}
