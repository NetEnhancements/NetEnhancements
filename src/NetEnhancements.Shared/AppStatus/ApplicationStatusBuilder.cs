using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace NetEnhancements.Shared.AppStatus
{
    public static class ApplicationStatusBuilder
    {
        public static ApplicationStatus FromHostedAssembly(IHostEnvironment hostingEnvironment, bool includingSensitiveData = false)
        {
            var assembly = GetEntryAssembly();

            var version = FileVersionInfo.GetVersionInfo(assembly.Location);

            var result = new ApplicationStatus
            (
                assembly.GetName()!.Name!,
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

        public static DateTime GetBuildDateTime(Assembly? assembly = null)
        {
            return System.IO.File.GetLastWriteTime((assembly ?? GetEntryAssembly()).Location);
        }

        private static Assembly GetEntryAssembly() => Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("Entry Assembly not found!");

    }
}
