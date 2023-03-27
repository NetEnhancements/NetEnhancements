using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace NetEnhancements.AspNet.Conventions
{
    /// <summary>
    /// Apply to a controller to auto-route its area based on its namespace.
    ///
    /// So instead of [Area("Foo")], use [AreaRouting] on a controller in the MyProject.Areas.Foo.Controllers namespace, inferring "Foo".
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AreaRoutingAttribute : Attribute, IControllerModelConvention
    {
        /// <inheritdoc/>
        public void Apply(ControllerModel controller)
        {
            controller.RouteAreaNamespace();
        }
    }
}
