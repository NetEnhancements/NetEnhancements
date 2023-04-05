namespace NetEnhancements.Shared.Configuration
{
    /// <summary>
    /// We need this class to read MS's Logging config, to be able to inject it.
    /// </summary>
    public class LoggingSettings
    {
        // This is not a setting.
        public const string Log4NetSectionName = "Log4Net";
    }
}
