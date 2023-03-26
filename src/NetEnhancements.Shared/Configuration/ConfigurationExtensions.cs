using System.Linq.Expressions;
using NetEnhancements.Util;
using Microsoft.Extensions.Configuration;

namespace NetEnhancements.Shared.Configuration
{
    /// <summary>
    /// Extension method container for configuration.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Returns the name of the type, with the suffix "Settings" removed if present.
        /// </summary>
        public static string GetSectionName<T>() => GetSectionName(typeof(T).Name);

        private static string GetSectionName(string name) => name.RemoveEnd("Settings");

        /// <summary>
        /// Get a <typeparamref name="TSettings"/> from an equally-named section in the appsettings, and the section so caller can register it for <see cref="Microsoft.Extensions.Options.IOptions{T}"/>.
        /// 
        /// <paramref name="sectionName"/> is implied from the name of <typeparamref name="TSettings"/>, with the suffix "Settings" removed, if present.
        /// </summary>
        public static (IConfigurationSection, TSettings) GetSectionOrThrow<TSettings>(this IConfiguration configuration, string? sectionName = null)
            where TSettings : class
        {
            var (section, usedSectionName) = configuration.GetSection<TSettings>(sectionName);

            TSettings settingsObject = section.Get<TSettings>() ?? throw new ArgumentException($"No '{usedSectionName}' section found in the configuration file", nameof(configuration));

            ValidateByDataAnnotation(settingsObject, section.Key);

            return (section, settingsObject);
        }

        /// <summary>
        /// Validates a settings instance by its annotations, throws if invalid.
        /// </summary>
        public static void ValidateByDataAnnotation<TSettings>(TSettings instance, string sectionName)
            where TSettings : class
        {
            var (isValid, validationResults) = AttributeValidator.Validate(instance);

            if (!isValid)
            {
                var propertyErrors = validationResults.Select(r => r.ErrorMessage);

                throw new ConfigurationException($"Invalid configuration for section '{sectionName}': \r\n * {string.Join("\r\n * ", propertyErrors)}");
            }
        }

        /// <summary>
        /// Get a named section in the appsettings, where <paramref name="sectionName"/> is implied from the name of <typeparamref name="T"/>, with the suffix "Settings" removed, if present.
        /// </summary>
        private static (IConfigurationSection, string sectionName) GetSection<T>(this IConfiguration configuration, string? sectionName = null)
        {
            sectionName = sectionName == null 
                ? GetSectionName<T>() 
                : GetSectionName(sectionName);

            return (configuration.GetSection(sectionName), sectionName);
        }

        private static (string MemberName, TValue? Value) GetSettingsValue<TSettings, TValue>(TSettings settingsValue, Expression<Func<TSettings, TValue>> propertySelector)
            where TSettings : class
        {
            if (propertySelector.NodeType != ExpressionType.Lambda || propertySelector.Body is not MemberExpression memberExpression)
            {
                throw new ArgumentException("Expression does not point to a property name", nameof(propertySelector));
            }

            if (settingsValue == null)
            {
                // Entire section missing
                throw ConfigurationException.MissingSection<TSettings>();
            }

            var compiledExpression = propertySelector.Compile();

            TValue propertyValue = compiledExpression.Invoke(settingsValue);
            
            return (memberExpression.Member.Name, propertyValue);
        }
    }
}
