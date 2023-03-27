namespace NetEnhancements.Util.Json;

/// <summary>
/// A converter for <see cref="TimeOnly"/> with custom format support.
/// </summary>
/// <example>
/// <code>
/// builder.Services.AddMvc()
///    .AddJsonOptions(options =>
///    {
///        // Custom TimeOnly formats.
///        options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter("HH:mm", "HHmm", "HH:mm:ss.fff"));
///    });
/// </code>
/// </example>
public class TimeOnlyConverter : ParsingConverter<TimeOnly>
{
    /// <inheritdoc/>
    public TimeOnlyConverter(string defaultFormat = "HH:mm:ss.fff", params string[] serializationFormats)
        : base(defaultFormat, serializationFormats)
    {
    }

    /// <inheritdoc/>
    protected override string ToStringValue(TimeOnly value) => value.ToString(SerializationFormats[0]);

    /// <inheritdoc/>
    protected override TimeOnly FromStringValue(string value)
    {
        if (!TimeOnly.TryParseExact(value, SerializationFormats, out var time))
        {
            ThrowUnparsable(value);
        }

        return time;
    }
}