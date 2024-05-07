using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetEnhancements.AspNet.Conventions
{
    /// <summary>
    /// Configures an area policy using an area name and zero or more filters.
    /// </summary>
    /// <param name="AreaName">The name of the area to apply the filters to. Use <see cref="DefaultAreaPolicy.PolicyName"/> to indicate any unspecified area (including no area).</param>
    /// <param name="Filters">The filter(s) to apply, if any.</param>
    public record AreaPolicy(string AreaName, params IFilterMetadata[] Filters);

    /// <summary>
    /// Registers filters for the "default area policy", i.e. not applicable to any other registered policy.
    /// </summary>
    public record DefaultAreaPolicy() : AreaPolicy(PolicyName, new AuthorizeFilter(PolicyName))
    {
        /// <summary>
        /// Placeholder name to recognize the default area policy being applied.
        /// </summary>
        public const string PolicyName = "(DefaultAreaPolicy)";
    }

    /// <summary>
    /// Applies the provided policies to MVC Controllers and Razor Pages.
    /// </summary>
    public class AreaAuthorizationPolicyConvention : IControllerModelConvention, IPageApplicationModelConvention
    {
        private readonly IFilterMetadata[] _defaultPolicyFilters;

        private readonly ReadOnlyDictionary<string, AreaPolicy> _policies;

        /// <summary>
        /// Instantiates the convention with the given policies.
        /// </summary>
        /// <param name="policies"></param>
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor - so user can use `new()`.
        public AreaAuthorizationPolicyConvention(IReadOnlyCollection<AreaPolicy> policies)
        {
            ValidatePolicies(policies);
            
            _policies = policies.ToDictionary(p => p.AreaName).AsReadOnly();

            _policies.TryGetValue(DefaultAreaPolicy.PolicyName, out var defaultPolicy);

            _defaultPolicyFilters = defaultPolicy?.Filters ?? [];
        }

        [StackTraceHidden]
        private static void ValidatePolicies(IReadOnlyCollection<AreaPolicy> policies)
        {
            if (policies.DistinctBy(p => p.AreaName).Count() != policies.Count)
            {
                throw new ArgumentException("Area Policy area names must be unique", nameof(policies));
            }
        }

        /// <inheritdoc/>
        public void Apply(ControllerModel controller)
        {
            controller.RouteValues.TryGetValue("area", out var areaName);

            ApplyPolicy(areaName, controller.Filters);
        }

        /// <inheritdoc/>
        public void Apply(PageApplicationModel model)
        {
            ApplyPolicy(model.AreaName, model.Filters);
        }

        private void ApplyPolicy(string? areaName, ICollection<IFilterMetadata> filters)
        {
            var wasConfigured = _policies.TryGetValue(areaName ?? DefaultAreaPolicy.PolicyName, out var areaPolicy);

            var policyFilters = areaPolicy?.Filters ?? _defaultPolicyFilters;

            Debug.WriteLine($"Using {(wasConfigured ? "configured" : "default")} policy with {policyFilters.Length} filter{(policyFilters.Length == 1 ? "" : "s")} for area {areaName}");

            foreach (var filter in policyFilters)
            {
                filters.Add(filter);
            }
        }
    }
}
