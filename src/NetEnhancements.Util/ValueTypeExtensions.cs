using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NetEnhancements.Util
{
    public static class ValueTypeExtensions
    {
        public static string ToValueString(this decimal value) =>
            value.HasDecimals()
                ? value.ToString(".00################")
                : value.ToString("0");

        public static string ToValueString(this decimal? value) =>
            value.HasValue
                ? value.Value.ToValueString()
                : "0";

        /// <summary>
        /// TODO: yeah. Which currency is this anyway?
        /// </summary>
        public static string ToPriceString(this decimal value, int decimals = 2, IFormatProvider? cultureInfo = null)
            => ((decimal?)value).ToPriceString(decimals, cultureInfo);

        /// <summary>
        /// TODO: yeah. Which currency is this anyway?
        /// </summary>
        public static string ToPriceString(this decimal? value, int decimals = 2, IFormatProvider? cultureInfo = null) =>
            value.HasValue
                ? value.Value.ToString($"N{decimals}", cultureInfo)
                : "-";

        public static bool HasDecimals(this decimal d) => d % 1 != 0;

        //TODO: Humanizer?
        public static string? ToReadableString(this DateTimeOffset? dateTime) => dateTime?.ToReadableString();

        /// <summary>
        /// Readable date string: "today, HH:mm:ss", "yesterday, HH:mm:ss", "Jan 01, HH:mm:ss", ...
        /// </summary>
        public static string ToReadableString(this DateTimeOffset dateTime)
        {
            if (dateTime.Date == DateTimeOffset.Now.Date)
            {
                return "vandaag, " + dateTime.ToString("HH:mm:ss");
            }

            if (dateTime.Date == DateTimeOffset.Now.AddDays(-1).Date)
            {
                return "gisteren, " + dateTime.ToString("HH:mm:ss");
            }

            // In the past 10 months or so, don't show the year.
            if (dateTime < DateTime.Now.AddMonths(-10) || dateTime.Year == DateTime.Now.Year)
            {
                return dateTime.ToString("MMM dd, HH:mm:ss");
            }

            return dateTime.ToString("MMM dd yyyy, HH:mm:ss");
        }

        public static string DaysOrWeeksUntil(this DateOnly firstDay, DateOnly lastDay)
        {
            var numDays = (lastDay.DayNumber - firstDay.DayNumber) + 1;
            var numWeeks = numDays >= 7 ? (int)Math.Ceiling(numDays / 7f) : 0;

            return numWeeks switch
            {
                0 => numDays == 1 ? "1 dag" : numDays + " dagen",
                1 => "1 week",
                _ => numWeeks + " weken"
            };
        }

        public static string WeekNumersUntil(this DateOnly firstDay, DateOnly lastDay)
        {
            var startWeek = ISOWeek.GetWeekOfYear(firstDay.ToDateTime(default));
            var endWeek = ISOWeek.GetWeekOfYear(lastDay.ToDateTime(default));

            if (startWeek == endWeek)
            {
                return "Week " + startWeek + " " + lastDay.Year;
            }

            if (endWeek < startWeek)
            {
                return "Week " + startWeek + " " + firstDay.Year +
                    " t/m "
                    + endWeek + " " + lastDay.Year;
            }

            return "Week " + startWeek + " t/m " + endWeek + " " + lastDay.Year;
        }

        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dateTime.DayOfWeek - startOfWeek)) % 7;
            return dateTime.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Get occurrences per <see cref="DayOfWeek"/> between <see cref="from"/> and <see cref="until"/>.
        /// </summary>
        public static Dictionary<DayOfWeek, int> GetDayFrequency(this DateTime from, DateTime until)
        {
            var startDay = DateOnly.FromDateTime(from);
            var endDay = DateOnly.FromDateTime(until);

            return startDay.GetDayFrequency(endDay);
        }

        /// <summary>
        /// Get occurrences per <see cref="DayOfWeek"/> between <see cref="from"/> and <see cref="until"/>.
        /// </summary>
        public static Dictionary<DayOfWeek, int> GetDayFrequency(this DateOnly from, DateOnly until)
        {
            if (until < from)
            {
                throw new ArgumentException("Date until must be on or after from");
            }

            var dayDict = Enum.GetValues<DayOfWeek>().ToDictionary(d => d, _ => 0);

            for (var date = from; date <= until; date = date.AddDays(1))
            {
                dayDict[date.DayOfWeek] += 1;
            }

            return dayDict;
        }
    }
}
