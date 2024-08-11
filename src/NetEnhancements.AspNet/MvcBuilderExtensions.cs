using Microsoft.Extensions.DependencyInjection;
using NetEnhancements.AspNet.Routing;

namespace NetEnhancements.AspNet
{
    /// <summary>
    /// Inject MVC enhancements.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Registers the <see cref="IRouteDebugger"/>.
        /// </summary>
        public static IMvcBuilder AddRouteDebugger(this IMvcBuilder builder)
        {
            builder.Services.AddScoped<IRouteDebugger, RouteDebugger>();

            return builder;
        }
    }
}
