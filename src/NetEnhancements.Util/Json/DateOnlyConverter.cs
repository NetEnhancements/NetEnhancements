using System;
using System.Globalization;

namespace NetEnhancements.Util.Json;

public class DateOnlyConverter : ParsingConverter<DateOnly>
{
    public DateOnlyConverter(string defaultFormat = "yyyy-MM-dd", params string[] serializationFormats)
        : base(defaultFormat, serializationFormats)
    {
    }

    protected override string ToStringValue(DateOnly value) => value.ToString(SerializationFormats[0]);

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
