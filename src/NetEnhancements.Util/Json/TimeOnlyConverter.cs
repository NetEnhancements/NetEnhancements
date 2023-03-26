using System;

namespace NetEnhancements.Util.Json;

public class TimeOnlyConverter : ParsingConverter<TimeOnly>
{
    public TimeOnlyConverter(string defaultFormat = "HH:mm:ss.fff", params string[] serializationFormats)
        : base(defaultFormat, serializationFormats)
    {
    }

    protected override string ToStringValue(TimeOnly value) => value.ToString(SerializationFormats[0]);

    protected override TimeOnly FromStringValue(string value)
    {
        if (!TimeOnly.TryParseExact(value, SerializationFormats, out var time))
        {
            ThrowUnparsable(value);
        }

        return time;
    }
}