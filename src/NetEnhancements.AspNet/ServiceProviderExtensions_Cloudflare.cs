using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using IPNetwork = System.Net.IPNetwork;

namespace NetEnhancements.AspNet
{
    /// <summary>
    /// Extension methods container for Cloudflare proxying.
    /// </summary>
    /// <url>https://www.cloudflare.com/ips/</url>
    public static class ServiceProviderExtensions
    {
        private const ForwardedHeaders DefaultAllowedHeaders = ForwardedHeaders.XForwardedFor;

        private static readonly IPNetwork[] TrustedNetworks =
        {
            // IPv4 - https://www.cloudflare.com/ips-v4/#
            new(IPAddress.Parse("173.245.48.0"), 20),
            new(IPAddress.Parse("103.21.244.0"), 22),
            new(IPAddress.Parse("103.22.200.0"), 22),
            new(IPAddress.Parse("103.31.4.0"), 22),
            new(IPAddress.Parse("141.101.64.0"), 18),
            new(IPAddress.Parse("108.162.192.0"), 18),
            new(IPAddress.Parse("190.93.240.0"), 20),
            new(IPAddress.Parse("188.114.96.0"), 20),
            new(IPAddress.Parse("197.234.240.0"), 22),
            new(IPAddress.Parse("198.41.128.0"), 17),
            new(IPAddress.Parse("162.158.0.0"), 15),
            new(IPAddress.Parse("104.16.0.0"), 13),
            new(IPAddress.Parse("104.24.0.0"), 14),
            new(IPAddress.Parse("172.64.0.0"), 13),
            new(IPAddress.Parse("131.0.72.0"), 22),

            // IPv6 - https://www.cloudflare.com/ips-v6/#
            new(IPAddress.Parse("2400:cb00::"), 32),
            new(IPAddress.Parse("2606:4700::"), 32),
            new(IPAddress.Parse("2803:f800::"), 32),
            new(IPAddress.Parse("2405:b500::"), 32),
            new(IPAddress.Parse("2405:8100::"), 32),
            new(IPAddress.Parse("2a06:98c0::"), 29),
            new(IPAddress.Parse("2c0f:f248::"), 32),
        };

        /// <summary>
        /// Configure Cloudflare proxy IPs as safe to trust X-Forwarded-For headers. This is required to get the correct client IP address when using Cloudflare as a reverse proxy.
        ///
        /// After calling this method to configure, call <see cref="ForwardedHeadersExtensions.UseForwardedHeaders(Microsoft.AspNetCore.Builder.IApplicationBuilder)"/> (<c>app.UseForwardedHeaders()</c>) before any other middleware.
        /// </summary>
        public static IServiceCollection ConfigureCloudflareForwarding(this IServiceCollection services, ForwardedHeaders? forwardedHeaders = DefaultAllowedHeaders)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = forwardedHeaders ?? DefaultAllowedHeaders;

                foreach (var network in TrustedNetworks)
                {
                    options.KnownIPNetworks.Add(network);
                }
            });

            return services;
        }
    }
}
