using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace NetEnhancements.AspNet.Conventions
{
    public static class ControllerModelExtensions
    {
        /// <summary>
        /// Route this controller to an area based on their "Areas." namespace, if any. This prevents having to put [Area("Foo")] on it.
        /// </summary>
        public static void RouteAreaNamespace(this ControllerModel controller)
        {
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

            var template = areaName + "/[controller]/[action]/{id?}";
            controller.RouteValues.Add("area", areaName);

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = new AttributeRouteModel
                {
                    Template = template
                };
            }
        }
    }
}
