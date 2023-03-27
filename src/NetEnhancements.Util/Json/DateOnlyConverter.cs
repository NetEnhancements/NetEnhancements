using System.Globalization;

namespace NetEnhancements.Util.Json;

/// <summary>
/// A converter for <see cref="DateOnly"/> with custom format support.
/// </summary>
/// <example>
/// <code>
/// builder.Services.AddMvc()
///    .AddJsonOptions(options =>
///    {
///        // Custom DateOnly formats.
///        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter("yyyy-MM-dd", "dd-MM-yyyy", "yyyy-MM-dd'T'HH:mm:ss.fffK"));
///
///        // "" == null for numbers.
///        options.JsonSerializerOptions.Converters.Add(new EmptyStringToNullNumberConverterFactory());
///    });
/// </code>
/// </example>
public class DateOnlyConverter : ParsingConverter<DateOnly>
{
    /// <inheritdoc/>
    public DateOnlyConverter(string defaultFormat = "yyyy-MM-dd", params string[] serializationFormats)
        : base(defaultFormat, serializationFormats)
    {
    }

    /// <inheritdoc/>
    protected override string ToStringValue(DateOnly value) => value.ToString(SerializationFormats[0]);

    /// <inheritdoc/>
    protected override DateOnly FromStringValue(string value)
    {
        // Dates can be sent from JS with timezone info, which gets dropped by DateOnly.Parse().
        if (!DateTimeOffset.TryParseExact(value, SerializationFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeOffset))
        {
            ThrowUnparsable(value);
        }

        // Explicitly convert it to local time relative to the provided timezone.
        return DateOnly.FromDateTime(dateTimeOffset.ToLocalTime().DateTime);
    }
}
