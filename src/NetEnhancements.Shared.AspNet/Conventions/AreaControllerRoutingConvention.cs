using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace NetEnhancements.AspNet.Conventions
{
    /// <summary>
    /// Apply this convention to route all controllers based on their "Areas." namespace, if any.
    /// 
    /// This prevents having to put [Area("Foo")] on every controller.
    /// </summary>
    public class AreaControllerRoutingConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                controller.RouteAreaNamespace();
            }
        }
    }
}
