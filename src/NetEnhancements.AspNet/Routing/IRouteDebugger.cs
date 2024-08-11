namespace NetEnhancements.AspNet.Routing
{
    /// <summary>
    /// Contains functionality to debug the routing system.
    /// </summary>
    public interface IRouteDebugger
    {
        /// <summary>
        /// Retrieve all routes registered with ASP.NET.
        /// </summary>
        IReadOnlyCollection<RouteModel> GetRoutes();
    }
}