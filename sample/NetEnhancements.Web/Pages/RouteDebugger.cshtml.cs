using Microsoft.AspNetCore.Mvc.RazorPages;
using NetEnhancements.AspNet.Routing;

namespace NetEnhancements.Web.Pages
{
    public class RouteDebuggerModel : PageModel
    {
        private readonly ILogger<RouteDebuggerModel> _logger;
        private readonly IRouteDebugger _routeDebugger;

        public RouteDebuggerModel(ILogger<RouteDebuggerModel> logger, IRouteDebugger routeDebugger)
        {
            _logger = logger;
            _routeDebugger = routeDebugger;
        }

        public IReadOnlyCollection<RouteModel> ActionDescriptorRoutes { get; private set; } = Array.Empty<RouteModel>();

        public void OnGet()
        {
            ActionDescriptorRoutes = _routeDebugger.GetRoutes();
        }
    }
}