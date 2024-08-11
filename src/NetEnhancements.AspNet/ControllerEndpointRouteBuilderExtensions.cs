using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace NetEnhancements.AspNet
{
    /// <summary>
    /// Contains extension methods for using Controllers with <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    public static class ControllerEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds endpoints for controller actions to the <see cref="IEndpointRouteBuilder"/> and adds the default route
        /// <c>{area:exists}/{controller=Home}/{action=Index}/{id?}</c> for routing within areas, named "defaultAreas".
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <returns>
        /// A <see cref="ControllerActionEndpointConventionBuilder"/> for configuring endpoints associated with controller actions for this route.
        /// </returns>
        public static ControllerActionEndpointConventionBuilder MapDefaultAreaControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapControllerRoute(
                name: "defaultAreas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        }
    }
}
