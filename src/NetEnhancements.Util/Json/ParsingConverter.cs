using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetEnhancements.Util.Json;

/// <summary>
/// Converts value types to and from JSON by parsing them in the formats specified in the constructor.
/// </summary>
public abstract class ParsingConverter<T> : JsonConverter<T>
{
    /// <summary>
    /// Contains the registered serialization formats, to parse from and serialize into.
    /// </summary>
    protected readonly string[] SerializationFormats;

    /// <inheritdoc/>
    protected ParsingConverter(string defaultFormat, params string[] serializationFormats)
    {
        ArgumentNullException.ThrowIfNull(serializationFormats);

        SerializationFormats = new[] { defaultFormat }.Concat(serializationFormats).ToArray();
    }

    /// <inheritdoc/>
    public sealed override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            => writer.WriteStringValue(ToStringValue(value));

    /// <inheritdoc/>
    public sealed override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        return FromStringValue(value!);
    }

    /// <summary>
    /// Return a string representation of a <typeparamref name="T"/>. Should take the first format from the <see cref="SerializationFormats"/> collection.
    /// </summary>
    protected abstract string ToStringValue(T value);

    /// <summary>
    /// Parse a value into a <typeparamref name="T"/>.
    /// </summary>
    protected abstract T FromStringValue(string value);

    /// <summary>
    /// Throws a <see cref="JsonException"/> indicating the value could not be parsed.
    /// </summary>
    [DoesNotReturn]
    protected void ThrowUnparsable(string value)
        => throw new JsonException($"The JSON value '{value}' could not be converted to {typeof(T).FullName}");
}