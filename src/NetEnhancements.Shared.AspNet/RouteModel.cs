namespace NetEnhancements.AspNet
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
        /// Route name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Route template.
        /// </summary>
        public string? Template { get; set; }
    }
}
