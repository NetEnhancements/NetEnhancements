using NetEnhancements.Shared.AspNet.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;

namespace NetEnhancements.Shared.AspNet
{
    public static class MvcOptionsExtensions
    {
        public static void UseGeneralRoutePrefix<TControllerBase>(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
            where TControllerBase : Controller
        {
            opts.Conventions.Add(new RoutePrefixConvention<TControllerBase>(routeAttribute));
        }

        //public static void UseGeneralRoutePrefix(this RazorPagesOptions opts, IRouteTemplateProvider routeAttribute)
        //{
        //    opts.Conventions.Add(new PageModelRoutePrefixConvention(routeAttribute));
        //}

        /// <summary>
        /// Applies the <see cref="AreaControllerRoutingConvention"/> convention that routes controllers based on their "Areas." namespace, if any.
        /// </summary>
        public static void UseAreaControllerNamespace(this MvcOptions options)
        {
            options.Conventions.Add(new AreaControllerRoutingConvention());
        }

        public static void UseGeneralRoutePrefix<TControllerBase>(this MvcOptions options, string prefix)
            where TControllerBase : Controller
        {
            options.UseGeneralRoutePrefix<TControllerBase>(new RouteAttribute(prefix));
        }

        public static void UseGeneralRoutePrefix(this RazorPagesOptions options, string areaName, string prefix, bool removeAreaFromUrl = false)
        {
            options.Conventions.Add(new PageModelRoutePrefixConvention(areaName, prefix, removeAreaFromUrl));
        }
    }
}
