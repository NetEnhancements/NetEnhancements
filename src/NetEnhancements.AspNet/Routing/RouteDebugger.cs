using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace NetEnhancements.AspNet.Routing
{
    internal class RouteDebugger(IEnumerable<EndpointDataSource> endpointDataSources) : IRouteDebugger
    {
        public IReadOnlyCollection<RouteModel> GetRoutes()
        {
            return GetEndpointRoutes()
                .OrderBy(a => a.Area)
                .ThenBy(a => a.Name)
                .ToArray();
        }

        private IEnumerable<RouteModel> GetEndpointRoutes()
        {
            var endpoints = endpointDataSources.SelectMany(e => e.Endpoints);

            foreach (var e in endpoints)
            {
                var routeDiagnostics = e.Metadata.OfType<IRouteDiagnosticsMetadata>().FirstOrDefault();

                var controllerAction = e.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();
                if (controllerAction != null)
                {
                    yield return MapController(e, controllerAction, routeDiagnostics);
                    continue;
                }

                var pageAction = e.Metadata.OfType<CompiledPageActionDescriptor>().FirstOrDefault();
                if (pageAction != null)
                {
                    yield return MapPage(e, pageAction, routeDiagnostics);
                    continue;
                }
                
                var endpointName = e.Metadata.OfType<EndpointNameMetadata>().FirstOrDefault()?.EndpointName;
                if (string.IsNullOrWhiteSpace(endpointName))
                {
                    if (routeDiagnostics == null)
                    {
                        // What's this?
                        continue;
                    }

                    endpointName = "(Anonymous minimal API)";
                }

                yield return new RouteModel
                {
                    Area = "",
                    Name = endpointName,
                    Template = routeDiagnostics?.Route,
                    Methods = GetMethods(e)
                };
            }
        }

        private static RouteModel MapController(Endpoint endpoint, ControllerActionDescriptor controllerAction, IRouteDiagnosticsMetadata? routeDiagnostics)
        {
            if (!controllerAction.RouteValues.TryGetValue("area", out var area) || string.IsNullOrWhiteSpace(area))
            {
                area = "(none)";
            }

            return new RouteModel
            {
                Area = area,
                Name = GetActionDescriptorName(controllerAction),
                Template = "",
                Methods = GetMethods(endpoint),
            };
        }

        private static RouteModel MapPage(Endpoint endpoint, CompiledPageActionDescriptor pageAction, IRouteDiagnosticsMetadata? routeDiagnostics)
        {
            var pageRoute = endpoint.Metadata.OfType<PageRouteMetadata>().FirstOrDefault();
            
            var pageName = GetCompiledPageName(pageAction);

            if (string.IsNullOrWhiteSpace(pageName))
            {
                pageName = pageRoute?.PageRoute ?? pageAction.DisplayName;
            }
            return new RouteModel
            {
                Area = pageAction.AreaName ?? "(none)",
                Name = pageName,
                Template = pageRoute?.RouteTemplate ?? pageRoute?.PageRoute,
                Methods = GetMethods(endpoint),
            };
        }

        private static IReadOnlyList<string> GetMethods(Endpoint endpoint)
        {
            var httpMethods = endpoint.Metadata.OfType<HttpMethodMetadata>()
                .SelectMany(m => m.HttpMethods)
                .ToArray();

            return httpMethods.Length > 0 ? httpMethods : ["(all)"];
        }

        /// <summary>
        /// Return the name of the current controller or page.
        /// </summary>
        private static string GetActionDescriptorName(ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor is CompiledPageActionDescriptor compiledPageActionDescriptor)
            {
                var pageName = GetCompiledPageName(compiledPageActionDescriptor);

                if (!string.IsNullOrWhiteSpace(pageName))
                {
                    return pageName;
                }
            }
            
            return actionDescriptor.AttributeRouteInfo?.Name ?? actionDescriptor.DisplayName ?? "(None)";
        }

        /// <summary>
        /// Page handlers have a name like "DemoModel". This takes the full name including assembly name ("Foo.Bar.DemoModel (Foo.Bar)").
        /// </summary>
        private static string? GetCompiledPageName(CompiledPageActionDescriptor actionDescriptor)
        {
            var handlerType = actionDescriptor.HandlerTypeInfo;

            var handlerTypeName = handlerType.FullName;

            if (string.IsNullOrWhiteSpace(handlerTypeName))
            {
                return null;
            }

            var assemblyName = handlerType.Assembly.GetName().Name;

            return !string.IsNullOrWhiteSpace(assemblyName) ?
                $"{handlerTypeName} ({assemblyName})"
                : handlerTypeName;
        }
    }
}
