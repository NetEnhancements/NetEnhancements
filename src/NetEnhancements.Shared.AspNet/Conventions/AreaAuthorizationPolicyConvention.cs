using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetEnhancements.Shared.AspNet.Conventions
{
    public class DefaultAreaPolicyFilter : AuthorizeFilter
    {
        public DefaultAreaPolicyFilter() : base(DefaultAreaPolicy.PolicyName) { }
    }

    /// <summary>
    /// Registers filters for the "default area policy", i.e. not applicable to any other registered policy.
    /// </summary>
    public record DefaultAreaPolicy()
        : AreaPolicy(PolicyName, new DefaultAreaPolicyFilter())
    {
        public const string PolicyName = "(DefaultPolicy)";
    };

    public record AreaPolicy(string? AreaName, params IFilterMetadata[] Filters);

    /// <summary>
    /// Applies the provided policies to MVC Controllers and Razor Pages.
    /// </summary>
    public class AreaAuthorizationPolicyConvention : IControllerModelConvention, IPageApplicationModelConvention
    {

        private readonly IFilterMetadata[] _defaultPolicy;

        private readonly ICollection<AreaPolicy> _policies;

        // ReSharper disable once SuggestBaseTypeForParameterInConstructor - so user can use `new()`.
        public AreaAuthorizationPolicyConvention(List<AreaPolicy> policies)
        {
            ValidatePolicies(policies);

            _policies = policies;
            _defaultPolicy = _policies.FirstOrDefault(p => p.AreaName == DefaultAreaPolicy.PolicyName)?.Filters ?? Array.Empty<IFilterMetadata>();
        }

        [StackTraceHidden]
        private static void ValidatePolicies(List<AreaPolicy> policies)
        {
            if (policies.DistinctBy(p => p.AreaName).Count() != policies.Count)
            {
                throw new ArgumentException("Area Policy area names must be unique", nameof(policies));
            }
        }

        public void Apply(ControllerModel controller)
        {
            controller.RouteValues.TryGetValue("area", out var areaName);

            ApplyPolicy(areaName, controller.Filters);
        }

        public void Apply(PageApplicationModel model)
        {
            ApplyPolicy(model.AreaName, model.Filters);
        }

        private void ApplyPolicy(string? areaName, ICollection<IFilterMetadata> filters)
        {
            var areaPolicy = _policies.FirstOrDefault(p => p.AreaName == areaName)?.Filters;

            bool wasConfigured = areaPolicy != null;

            areaPolicy ??= _defaultPolicy;
            
            Debug.WriteLine($"Using {(wasConfigured ? "configured" : "default")} policy with {areaPolicy.Length} filter{(areaPolicy.Length == 1 ? "" : "s")} for area {areaName}");

            foreach (var filter in areaPolicy)
            {
                filters.Add(filter);
            }
        }
    }
}
