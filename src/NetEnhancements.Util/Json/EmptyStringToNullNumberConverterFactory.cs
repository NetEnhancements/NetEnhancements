﻿using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetEnhancements.Util.Json;

/// <summary>
/// Register this factory to let all nullable numeric types be conversible from empty string to null,
/// when the <see cref="JsonSerializerOptions"/>.<see cref="JsonSerializerOptions.NumberHandling"/> contains <see cref="JsonNumberHandling.AllowReadingFromString"/>.
/// </summary>
/// <example>
/// <code>
/// builder.Services.AddMvc()
///    .AddJsonOptions(options =>
///    {
///        // "" == null for numbers.
///        options.JsonSerializerOptions.Converters.Add(new EmptyStringToNullNumberConverterFactory());
///    });
/// </code>
/// </example>
public class EmptyStringToNullNumberConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// We can convert from and to nullable numeric types.
    /// 
    /// Gets called once per type per application run, no need for caching.
    /// </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        var type = Nullable.GetUnderlyingType(typeToConvert);

        return type?.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>)) == true;
    }

    /// <summary>
    /// Gets called once per type per application run, no need for caching.
    /// </summary>
    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var underlyingType = Nullable.GetUnderlyingType(type)!;

        var genericMethod = _factoryMethod.MakeGenericMethod(underlyingType);

        return (JsonConverter)genericMethod.Invoke(this, new object?[] { options })!;
    }

    // Do cache the factory method.
    private readonly MethodInfo _factoryMethod = typeof(EmptyStringToNullNumberConverterFactory).GetMethod(nameof(CreateConverterInternal), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static JsonConverter<TNumber?> CreateConverterInternal<TNumber>(JsonSerializerOptions options)
        where TNumber : struct, INumber<TNumber>
    {
        return new EmptyStringToNullNumberConverter<TNumber>(/* options */);
    }
}

/// <summary>
/// Converts a given number type from empty strings to null,
/// when the <see cref="JsonSerializerOptions"/>.<see cref="JsonSerializerOptions.NumberHandling"/> contains <see cref="JsonNumberHandling.AllowReadingFromString"/>.
///
/// Have this class factory-created by registering <see cref="EmptyStringToNullNumberConverterFactory"/>.
/// </summary>
public class EmptyStringToNullNumberConverter<TNumber> : JsonConverter<TNumber?>
    where TNumber : struct, INumber<TNumber>
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TNumber?);

    /// <inheritdoc/>
    public override TNumber? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // We won't be called for reading nulls, but will be for empty strings.
        if (reader.TokenType == JsonTokenType.String
            && options.NumberHandling.HasFlag(JsonNumberHandling.AllowReadingFromString)
            && reader.ValueTextEquals(ReadOnlySpan<byte>.Empty))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TNumber>(ref reader, options);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TNumber? value, JsonSerializerOptions options)
        // We won't be called for nulls.
        => JsonSerializer.Serialize(writer, value!.Value, options);
}
