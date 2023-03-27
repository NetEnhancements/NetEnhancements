using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace NetEnhancements.Shared.AppStatus
{
    /// <summary>
    /// Reads application (main assembly) information.
    /// </summary>
    public static class ApplicationStatusBuilder
    {
        /// <summary>
        /// Returns host and version information about the executed assembly.
        /// </summary>
        public static ApplicationStatus FromHostedAssembly(IHostEnvironment hostingEnvironment, bool includingSensitiveData = false)
        {
            var assembly = GetEntryAssembly();

            var version = FileVersionInfo.GetVersionInfo(assembly.Location);

            var result = new ApplicationStatus
            (
                assembly.GetName().Name!,
                version.ProductVersion!,
                hostingEnvironment.EnvironmentName
            );

            if (includingSensitiveData)
            {
                result.AssemblyLocation = assembly.Location;
                result.Hostname = Environment.MachineName;
                result.AssemblyLastWrite = GetBuildDateTime(assembly);
            }

            return result;
        }

        /// <summary>
        /// Returns when an assembly was last written.
        /// </summary>
        public static DateTime GetBuildDateTime(Assembly? assembly = null)
        {
            return File.GetLastWriteTime((assembly ?? GetEntryAssembly()).Location);
        }

        private static Assembly GetEntryAssembly() => Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("Entry Assembly not found!");

    }
}
