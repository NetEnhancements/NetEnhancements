namespace NetEnhancements.Shared.Configuration
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string? message = null, Exception? innerException = null)
            : base(message, innerException)
        {
        }

        public static ConfigurationException MissingSection<TSettings>()
        where TSettings : class
        {
            var sectionName = ConfigurationExtensions.GetSectionName<TSettings>();

            return new ConfigurationException($"Configuration section '{sectionName}' missing");
        }

        public static ConfigurationException EmptyString<TSettings>(string memberName)
            where TSettings : class
        {
            var sectionName = ConfigurationExtensions.GetSectionName<TSettings>();

            return new ConfigurationException($"AppSettings value '{sectionName}.{memberName}' may not be an empty string");
        }

        public static ConfigurationException DefaultValue<TSettings>(string memberName, object value)
            where TSettings : class
        {
            var sectionName = ConfigurationExtensions.GetSectionName<TSettings>();

            return new ConfigurationException($"AppSettings value '{sectionName}.{memberName}' may not be a default value ('{value}').");
        }
    }
}
