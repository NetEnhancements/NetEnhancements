using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetEnhancements.Shared.AppStatus
{
    /// <summary>
    /// Represents the status of a deployed application.
    /// </summary>
    public class ApplicationStatus
    {
        /// <summary>
        /// Construct the status with the required properties.
        /// </summary>
        public ApplicationStatus(string assemblyName, string assemblyVersion, string environmentName)
        {
            AssemblyName = assemblyName;
            AssemblyVersion = assemblyVersion;
            NetCoreHostingEnvironment = environmentName;
        }

        /// <summary>
        /// The main application assembly name.
        /// </summary>
        [Display(Name = "Assembly")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// The main application assembly version.
        /// </summary>
        [Display(Name = "Version")]
        public string AssemblyVersion { get; set; }

        /// <summary>
        /// The application's runtime configuration.
        /// </summary>
        // Named "environment" for public API to not leak any details about the fact we're running on (ASP).NET (Core).
        [JsonPropertyName("environment")]
        [Display(Name = ".NET Core environment")]
        public string NetCoreHostingEnvironment { get; set; }

        // Optional properties
        /// <summary>
        /// Where the main application assembly lives.
        /// </summary>
        [Display(Name = "Location")]
        public string? AssemblyLocation { get; set; }

        /// <summary>
        /// When the main application assembly was last written.
        /// </summary>
        [Display(Name = "Last written")]
        public DateTime? AssemblyLastWrite { get; set; }

        /// <summary>
        /// The machine name on which the application is running.
        /// </summary>
        [Display(Name = "Hostname")]
        public string? Hostname { get; set; }
    }
}
