using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace NetEnhancements.Shared.AspNet.Conventions
{
    public class RoutePrefixConvention<TControllerBase> : IApplicationModelConvention
        where TControllerBase : Controller
    {
        private readonly Type _baseControllerType;
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefixConvention(IRouteTemplateProvider route)
        {
            _routePrefix = new AttributeRouteModel(route);
            _baseControllerType = typeof(TControllerBase);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers.Where(x => _baseControllerType.IsAssignableFrom(x.ControllerType)).SelectMany(c => c.Selectors))
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel != null 
                    ? AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel) 
                    : _routePrefix;
            }
        }
    }

    /// <summary>
    /// Apply to update or remove an area route prefix.
    /// </summary>
    public class PageModelRoutePrefixConvention : IPageRouteModelConvention
    {
        private readonly string _areaName;
        private readonly string _prefix;
        private readonly bool _removeAreaFromUrl;

        public PageModelRoutePrefixConvention(string areaName, string prefix, bool removeAreaFromUrl)
        {
            _areaName = areaName;
            _prefix = prefix;
            _removeAreaFromUrl = removeAreaFromUrl;
        }

        public void Apply(PageRouteModel model)
        {
            if (model.AreaName != _areaName)
            {
                return;
            }

            var selector = model.Selectors.First();
            model.Selectors.Clear();

            var template = selector.AttributeRouteModel?.Template;
            if (_removeAreaFromUrl)
            {
                template = selector.AttributeRouteModel?.Template?.Replace(_areaName + "/", "");
            }

            if (template != null)
            {
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Order = 0,
                        Template = AttributeRouteModel.CombineTemplates(_prefix, template)
                    }
                });
            }
        }
    }
}
