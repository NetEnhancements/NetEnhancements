using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace NetEnhancements.AspNet.Conventions
{
    /// <summary>
    /// Extension methods for <see cref="ControllerModel"/>.
    /// </summary>
    public static class ControllerModelExtensions
    {
        private const string AreaRouteKey = "area";

        /// <summary>
        /// Route this controller to an area based on their "Areas." namespace, if any. This prevents having to put [Area("Foo")] on it.
        /// </summary>
        public static void RouteAreaNamespace(this ControllerModel controller)
        {
            if (controller.RouteValues.TryGetValue(AreaRouteKey, out var _))
            {
                return;
            }

            var hasRouteAttributes = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);
            if (hasRouteAttributes)
            {
                return;
            }

            var controllerTypeNamespace = controller.ControllerType.Namespace;
            if (controllerTypeNamespace == null)
            {
                return;
            }

            var areaName = controllerTypeNamespace.Split('.').SkipWhile(segment => segment != "Areas").Skip(1).FirstOrDefault();
            if (string.IsNullOrEmpty(areaName))
            {
                return;
            }

            // This is what the [Area] attribute ultimately does.
            controller.RouteValues.Add(AreaRouteKey, areaName);
        }
    }
}
