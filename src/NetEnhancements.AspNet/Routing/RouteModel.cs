namespace NetEnhancements.AspNet.Routing
{
    /// <summary>
    /// An ASP.NET Core route.
    /// </summary>
    public class RouteModel
    {
        /// <summary>
        /// The area the route points to.
        /// </summary>
        public string? Area { get; set; }

        /// <summary>
        /// Route, page or controller name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Route template.
        /// </summary>
        public string? Template { get; set; }

        /// <summary>
        /// The HTTP methods through which the route is accessible; all if empty.
        /// </summary>
        public IReadOnlyList<string> Methods { get; set; } = Array.Empty<string>();
    }
}
