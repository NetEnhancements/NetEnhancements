using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetEnhancements.AspNet.Conventions
{
    /// <summary>
    /// Represents the default policy, applied to all unspecified areas (including no area).
    /// </summary>
    public class DefaultAreaPolicyFilter : AuthorizeFilter
    {
        /// Instantiate the default policy, applied to all unspecified areas (including no area).
        public DefaultAreaPolicyFilter() : base(DefaultAreaPolicy.PolicyName) { }
    }

    /// <summary>
    /// Registers filters for the "default area policy", i.e. not applicable to any other registered policy.
    /// </summary>
    public record DefaultAreaPolicy()
        : AreaPolicy(PolicyName, new DefaultAreaPolicyFilter())
    {
        /// <summary>
        /// Placeholder name to recognize the default policy being applied.
        /// </summary>
        public const string PolicyName = "(DefaultPolicy)";
    }

    /// <summary>
    /// Configures an area policy using an area name and zero or more filters.
    /// </summary>
    /// <param name="AreaName">The name of the area to apply the filters to. <c>null</c> means no area.</param>
    /// <param name="Filters">The filter(s) to apply, if any.</param>
    public record AreaPolicy(string? AreaName, params IFilterMetadata[] Filters);

    /// <summary>
    /// Applies the provided policies to MVC Controllers and Razor Pages.
    /// </summary>
    public class AreaAuthorizationPolicyConvention : IControllerModelConvention, IPageApplicationModelConvention
    {
        private readonly IFilterMetadata[] _defaultPolicy;

        private readonly IReadOnlyList<AreaPolicy> _policies;

        /// <summary>
        /// Instantiates the convention with the given policies.
        /// </summary>
        /// <param name="policies"></param>
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor - so user can use `new()`.
        public AreaAuthorizationPolicyConvention(List<AreaPolicy> policies)
        {
            ValidatePolicies(policies);

            _policies = policies;
            _defaultPolicy = _policies.FirstOrDefault(p => p.AreaName == DefaultAreaPolicy.PolicyName)?.Filters ?? [];
        }

        [StackTraceHidden]
        private static void ValidatePolicies(List<AreaPolicy> policies)
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
            var areaPolicy = _policies.FirstOrDefault(p => p.AreaName == areaName)?.Filters;

            var wasConfigured = areaPolicy != null;

            areaPolicy ??= _defaultPolicy;

            Debug.WriteLine($"Using {(wasConfigured ? "configured" : "default")} policy with {areaPolicy.Length} filter{(areaPolicy.Length == 1 ? "" : "s")} for area {areaName}");

            foreach (var filter in areaPolicy)
            {
                filters.Add(filter);
            }
        }
    }
}
