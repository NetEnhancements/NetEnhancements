using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetEnhancements.Util.Json;

public abstract class ParsingConverter<T> : JsonConverter<T>
{
    protected readonly string[] SerializationFormats;

    protected ParsingConverter(string defaultFormat, params string[] serializationFormats)
    {
        ArgumentNullException.ThrowIfNull(serializationFormats);

        SerializationFormats = new[] { defaultFormat }.Concat(serializationFormats).ToArray();
    }

    public sealed override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        => writer.WriteStringValue(ToStringValue(value));

    public sealed override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        return FromStringValue(value!);
    }

    protected abstract string ToStringValue(T value);

    protected abstract T FromStringValue(string value);

    [DoesNotReturn]
    protected void ThrowUnparsable(string value)
        => throw new JsonException($"The JSON value '{value}' could not be converted to {typeof(T).FullName}");
}