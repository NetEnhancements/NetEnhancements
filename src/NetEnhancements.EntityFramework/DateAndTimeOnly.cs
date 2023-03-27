using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NetEnhancements.Shared.EntityFramework
{
    /// <summary>
    /// Entity Framework doesn't support DateOnly/TimeOnly natively.
    /// 
    /// https://github.com/dotnet/efcore/issues/24507
    /// https://github.com/dotnet/SqlClient/issues/1009
    /// </summary>
    public static class ModelConfigurationBuilderExtensions
    {
        public static ModelConfigurationBuilder AddDateOnly(this ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            builder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");

            return builder;
        }

        public static ModelConfigurationBuilder AddTimeOnly(this ModelConfigurationBuilder builder)
        {
            builder.Properties<TimeOnly>()
                .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>();

            builder.Properties<TimeOnly?>()
                .HaveConversion<NullableTimeOnlyConverter, NullableTimeOnlyComparer>();

            return builder;
        }
    }

    /// <summary> 
    /// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa. 
    /// </summary> 
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary> 
        /// Creates a new instance of this converter. 
        /// </summary> 
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }

    /// <summary> 
    /// Compares <see cref="DateOnly" />. 
    /// </summary> 
    public class DateOnlyComparer : ValueComparer<DateOnly>
    {
        /// <summary> 
        /// Creates a new instance of this converter. 
        /// </summary> 
        public DateOnlyComparer() : base(
            (d1, d2) => d1 == d2 && d1.DayNumber == d2.DayNumber,
            d => d.GetHashCode())
        {
        }
    }

    /// <summary>
    /// Converts nullable <see cref="DateOnly" /> to nullable <see cref="DateTime"/> and vice versa.
    /// </summary>
    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        /// <summary> 
        /// Creates a new instance of this converter. 
        /// </summary> 
        public NullableDateOnlyConverter() : base(
            d => d == null
                ? null
                : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
            d => d == null
                ? null
                : new DateOnly?(DateOnly.FromDateTime(d.Value)))
        { }
    }

    /// <summary>
    /// Compares <see cref="Nullable{DateOnly}" />.
    /// </summary>
    public class NullableDateOnlyComparer : ValueComparer<DateOnly?>
    {
        /// <summary> 
        /// Creates a new instance of this converter. 
        /// </summary> 
        public NullableDateOnlyComparer() : base(
            (d1, d2) => d1 == d2 && d1.GetValueOrDefault().DayNumber == d2.GetValueOrDefault().DayNumber,
            d => d.GetHashCode())
        {
        }
    }

    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter() : base(
            timeOnly => timeOnly.ToTimeSpan(),
            timeSpan => TimeOnly.FromTimeSpan(timeSpan))
        {
        }
    }

    public class TimeOnlyComparer : ValueComparer<TimeOnly>
    {
        public TimeOnlyComparer() : base(
            (t1, t2) => t1.Ticks == t2.Ticks,
            t => t.GetHashCode())
        {
        }
    }

    public class NullableTimeOnlyConverter : ValueConverter<TimeOnly?, TimeSpan?>
    {
        public NullableTimeOnlyConverter() : base(
            timeOnly => timeOnly != null ? timeOnly.Value.ToTimeSpan() : (TimeSpan?)null,
            timeSpan => TimeOnly.FromTimeSpan(timeSpan ?? new TimeSpan()))
        {
        }
    }

    public class NullableTimeOnlyComparer : ValueComparer<TimeOnly?>
    {
        public NullableTimeOnlyComparer() : base(
            (t1, t2) => t1 == t2,
            t => t.GetHashCode())
        {
        }
    }
}
