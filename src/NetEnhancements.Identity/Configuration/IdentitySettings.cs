namespace NetEnhancements.Identity.Configuration
{
    public class IdentitySettings
    {
        /// <summary>
        /// The application secret, used in encryption.
        /// </summary>
        public string? Secret { get; set; }

        /// <summary>
        /// Where to store key files, so we can validate cookies across app pools and recycles.
        /// </summary>
        public string? KeyFilePath { get; set; }

        /// <summary>
        /// The name to encrypt data with.
        /// </summary>
        public string? ApplicationName { get; set; }

        /// <summary>
        /// Max age of a JWT.
        /// </summary>
        public int TokenLifetimeSeconds { get; set; }
    }
}
