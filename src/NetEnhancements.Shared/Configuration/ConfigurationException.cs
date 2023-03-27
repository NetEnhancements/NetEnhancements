namespace NetEnhancements.Shared.Configuration
{
    /// <summary>
    /// Indicates that the configuration cannot be parsed or contains invalid values.
    /// </summary>
    public class ConfigurationException : Exception
    {
        /// <summary>
        /// Manually construct the exception. Consider using one of the static methods instead.
        /// </summary>
        public ConfigurationException(string? message = null, Exception? innerException = null)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Indicates that a configuration is missing the section for the given settings class.
        /// </summary>
        public static ConfigurationException MissingSection<TSettings>()
            where TSettings : class
        {
            var sectionName = ConfigurationExtensions.GetSectionName<TSettings>();

            return new ConfigurationException($"Configuration section '{sectionName}' missing");
        }

        /// <summary>
        /// Indicates that the given member for the given settings class contains an empty value in the configuration.
        /// </summary>
        public static ConfigurationException EmptyString<TSettings>(string memberName)
            where TSettings : class
        {
            var sectionName = ConfigurationExtensions.GetSectionName<TSettings>();

            return new ConfigurationException($"AppSettings value '{sectionName}.{memberName}' may not be an empty string");
        }

        /// <summary>
        /// Indicates that the given member for the given settings class contains a default value in the configuration.
        /// </summary>
        public static ConfigurationException DefaultValue<TSettings>(string memberName, object value)
            where TSettings : class
        {
            var sectionName = ConfigurationExtensions.GetSectionName<TSettings>();

            return new ConfigurationException($"AppSettings value '{sectionName}.{memberName}' may not be a default value ('{value}').");
        }
    }
}
