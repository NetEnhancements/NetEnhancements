using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace NetEnhancements.Util.Json;

internal static class ShouldSerializeModifier
{
    private static readonly Type BoolType = typeof(bool);
    private const string ShouldSerialize = "ShouldSerialize";
    private static readonly ConcurrentDictionary<Type, Dictionary<string, MethodInfo>> ReflectionCache = new();

    internal static void Action(JsonTypeInfo jsonTypeInfo)
    {
        var shoulders = GetShouldersCache(jsonTypeInfo.Type);

        if (!shoulders.Any())
        {
            return;
        }

        foreach (var p in jsonTypeInfo.Properties)
        {
            if (p.AttributeProvider?.GetCustomAttributes(typeof(JsonIgnoreAttribute), inherit: true).Any() == true)
            {
                continue;
            }

            // Don't serialize ShouldSerializeXxx methods themselves.
            if (p.Name.StartsWith(ShouldSerialize))
            {
                p.ShouldSerialize = (_, _) => false;

                continue;
            }

            // No Should method found? Skip.
            if (!shoulders.TryGetValue(p.Name, out var method))
            {
                continue;
            }

            // Save an optionally already present action.
            var existingTest = p.ShouldSerialize;

            // Call the existing action first, if any, then ours.
            p.ShouldSerialize = (containingObject, propertyValue) =>
                (existingTest == null || existingTest(containingObject, propertyValue))
                &&
                (bool)method.Invoke(containingObject, null)!;
        }
    }

    /// <summary>
    /// Returns all ShouldSerializeXxx methods (or getter methods) for a given type.
    /// </summary>
    private static Dictionary<string, MethodInfo> GetShouldersCache(Type type)
    {
        if (ReflectionCache.TryGetValue(type, out var shoulders))
        {
            return shoulders;
        }

        shoulders = new Dictionary<string, MethodInfo>();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyName = property.Name;
            var shouldPropertyName = ShouldSerialize + propertyName;

            var shouldMethod = type.GetMethod(shouldPropertyName);
            if (shouldMethod != null && shouldMethod.ReturnType == BoolType)
            {
                shoulders[propertyName] = shouldMethod;
                continue;
            }

            var shouldGetter = type.GetProperty(shouldPropertyName)?.GetMethod;
            if (shouldGetter != null && shouldGetter.ReturnType == BoolType)
            {
                shoulders[propertyName] = shouldGetter;
            }
        }

        return ReflectionCache[type] = shoulders;
    }
}
