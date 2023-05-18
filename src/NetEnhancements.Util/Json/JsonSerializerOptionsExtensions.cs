using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace NetEnhancements.Util.Json;

/// <summary>
/// Adds ShouldSerialize functionality to the <see cref="JsonSerializerOptions"/>.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    /// Adds ShouldSerialize functionality to the <see cref="JsonSerializerOptions"/>.
    ///
    /// This lets an object determine at runtime which properties should be serialized. A <c>bool ShouldSerializeXxx</c> method or property returning a <c>bool</c> indicating whether the property <c>Xxx</c> should be serialized.
    /// </summary>
    public static JsonSerializerOptions WithShouldSerializeModifier(this JsonSerializerOptions options)
    {
        var resolver = options.TypeInfoResolver as DefaultJsonTypeInfoResolver;

        if (options.TypeInfoResolver != null && resolver == null)
        {
            const string errorString = nameof(WithShouldSerializeModifier) + "() can only be called on options with a null or " + nameof(DefaultJsonTypeInfoResolver) + " " + nameof(options.TypeInfoResolver);
            throw new InvalidOperationException(errorString);
        }

        if (resolver == null)
        {
            options.TypeInfoResolver = resolver = new DefaultJsonTypeInfoResolver();
        }

        resolver.Modifiers.Add(ShouldSerializeModifier.Action);

        return options;
    }
}