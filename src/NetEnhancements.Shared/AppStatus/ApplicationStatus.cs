using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetEnhancements.Shared.AppStatus
{
    public class ApplicationStatus
    {
        public ApplicationStatus(string assemblyName, string assemblyVersion, string environmentName)
        {
            AssemblyName = assemblyName;
            AssemblyVersion = assemblyVersion;
            NetCoreHostingEnvironment = environmentName;
        }

        [Display(Name = "Assembly")]
        public string AssemblyName { get; set; }

        [Display(Name = "Version")]
        public string AssemblyVersion { get; set; }

        // Named "environment" for public API to not leak any details about the runtime.
        [JsonPropertyName("environment")]
        [Display(Name = ".NET Core environment")]
        public string NetCoreHostingEnvironment { get; set; }

        // Optional properties

        [Display(Name = "Location")]
        public string? AssemblyLocation { get; set; }

        [Display(Name = "Last written")]
        public DateTime? AssemblyLastWrite { get; set; }

        [Display(Name = "Hostname")]
        public string? Hostname { get; set; }
    }
}
