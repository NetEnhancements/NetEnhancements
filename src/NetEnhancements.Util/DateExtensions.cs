using System.Globalization;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for <see cref="DateTimeOffset"/>, <see cref="DateTime"/>, <see cref="DateOnly"/> and <see cref="TimeSpan"/> objects.
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Rounds a DateTime object to the nearest TimeSpan.
        /// </summary>
        /// <param name="dt">The DateTime object to round.</param>
        /// <param name="ts">The TimeSpan to round to.</param>
        /// <returns>A new DateTime object rounded to the nearest TimeSpan.</returns>
        public static DateTime RoundToNearest(this DateTime dt, TimeSpan ts)
        {
            var delta = dt.Ticks % ts.Ticks;
            var roundUp = delta > ts.Ticks / 2;
            var offset = roundUp ? ts.Ticks : 0;

            return new DateTime(dt.Ticks + offset - delta, dt.Kind);
        }

        /// <summary>
        /// Rounds a TimeSpan object to the nearest TimeSpan.
        /// </summary>
        /// <param name="ts">The TimeSpan object to round.</param>
        /// <param name="roundTo">The TimeSpan to round to.</param>
        /// <returns>A new TimeSpan object rounded to the nearest TimeSpan.</returns>
        public static TimeSpan RoundToNearest(this TimeSpan ts, TimeSpan roundTo)
        {
            var ticks = (long)(Math.Round(ts.Ticks / (double)roundTo.Ticks) * roundTo.Ticks);
            return new TimeSpan(ticks);
        }

        /// <summary>
        /// Determines if a DateTime object falls between two specified DateTime objects.
        /// </summary>
        /// <param name="dt">The DateTime object to check.</param>
        /// <param name="start">The start DateTime object to compare with.</param>
        /// <param name="end">The end DateTime object to compare with.</param>
        /// <returns>True if the DateTime object is between the start and end dates, inclusive; otherwise, false.</returns>
        public static bool IsBetweenTwoDates(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }

        /// <summary>
        /// Determines if two date ranges overlap.
        /// </summary>
        /// <param name="aStart">The start DateTime of the first date range.</param>
        /// <param name="aEnd">The end DateTime of the first date range.</param>
        /// <param name="bStart">The start DateTime of the second date range.</param>
        /// <param name="bEnd">The end DateTime of the second date range.</param>
        /// <returns>True if the date ranges overlap; otherwise, false.</returns>
        public static bool DoDatesOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
        {
            return aStart < bEnd && bStart < aEnd;
        }

        /// <summary>
        /// Converts a Unix time to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The Unix time to convert, in seconds since the Unix epoch (1970-01-01 00:00:00 UTC).</param>
        /// <returns>A DateTime object representing the converted Unix time.</returns>
        public static DateTime FromUnixTime(in long unixTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Converts a DateTime object to Unix time.
        /// </summary>
        /// <param name="dt">The DateTime object to convert.</param>
        /// <returns>A long value representing the Unix time.</returns>
        public static long ToUnixTime(this DateTime dt)
        {
            if (dt == DateTime.MinValue)
            {
                return 0;
            }
            var utcNow = new DateTimeOffset(dt);
            return utcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Generates an enumerable sequence of DateTime objects, one for each day in the specified range.
        /// </summary>
        /// <param name="from">The start date of the range, inclusive.</param>
        /// <param name="thru">The end date of the range, inclusive.</param>
        /// <returns>An IEnumerable&lt;DateTime&gt; containing one DateTime object for each day in the specified range.</returns>
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        /// <summary>
        /// Gets the start of the previous month for a given DateTime.
        /// </summary>
        /// <param name="dt">The DateTime object to get the start of the previous month from.</param>
        /// <returns>A new DateTime object representing the start of the previous month.</returns>
        public static DateTime StartOfPreviousMonth(this DateTime dt)
        {
            var previousMonth = dt.AddMonths(-1);
            return new DateTime(previousMonth.Year, previousMonth.Month, 1);
        }

        /// <summary>
        /// Gets the end of the previous month for a given DateTime.
        /// </summary>
        /// <param name="dt">The DateTime object to get the end of the previous month from.</param>
        /// <returns>A new DateTime object representing the end of the previous month.</returns>
        public static DateTime EndOfPreviousMonth(this DateTime dt)
        {
            return dt.StartOfPreviousMonth().AddMonths(1).AddTicks(-1);
        }

        /// <summary>
        /// Gets the first occurrence of a specified day of the week in a given month.
        /// </summary>
        /// <param name="date">The DateTime object representing the month to find the first occurrence of the day of the week.</param>
        /// <param name="firstDayOfWeek">The DayOfWeek value to find the first occurrence of in the month.</param>
        /// <returns>A new DateTime object representing the first occurrence of the specified day of the week in the given month.</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            var currentMonth = new DateTime(date.Year, date.Month, 1);

            while (currentMonth.DayOfWeek != firstDayOfWeek)
            {
                currentMonth = currentMonth.AddDays(1);
            }

            return currentMonth;
        }

        /// <summary>
        /// Converts a TimeSpan object into a human-readable string representation.
        /// </summary>
        /// <param name="ts">The TimeSpan object to humanize.</param>
        /// <returns>A string representing the human-readable version of the TimeSpan object.</returns>
        public static string Humanize(this TimeSpan ts)
        {
            var formatString = string.Empty;

            if (ts.Days > 0)
            {
                formatString += "d\\d\\ ";
            }

            if (ts.Hours > 0)
            {
                formatString += "h\\u\\ ";
            }

            if (ts.Minutes > 0)
            {
                formatString += "m\\m\\ ";
            }

            if (ts.Seconds > 0)
            {
                formatString += "s\\s\\ ";
            }

            if (ts.Milliseconds > 0)
            {
                formatString += "fff\\m\\s";
            }

            var result = ts.ToString(formatString).Trim().Replace(" 0", " ").Replace(" 0", " ");
            if (result.StartsWith('0'))
            {
                result = result[1..];
            }

            return result;
        }

        /// <summary>
        /// Readable date string: "today, HH:mm:ss", "yesterday, HH:mm:ss", "Jan 01, HH:mm:ss", ...
        /// </summary>
        public static string? ToReadableString(this DateTimeOffset? dateTime, string today = "today", string yesterday = "yesterday") => dateTime?.ToReadableString(today, yesterday);

        /// <summary>
        /// Readable date string: "today, HH:mm:ss", "yesterday, HH:mm:ss", "Jan 01, HH:mm:ss", ...
        /// </summary>
        public static string ToReadableString(this DateTimeOffset dateTime, string today = "today", string yesterday = "yesterday")
        {
            if (dateTime.Date == DateTimeOffset.Now.Date)
            {
                return today + ", " + dateTime.ToString("HH:mm:ss");
            }

            if (dateTime.Date == DateTimeOffset.Now.AddDays(-1).Date)
            {
                return yesterday + ", " + dateTime.ToString("HH:mm:ss");
            }

            // In the past 10 months or for dates in the current year, don't show the year.
            if (dateTime > DateTime.Now.AddMonths(-10) || dateTime.Year == DateTime.Now.Year)
            {
                return dateTime.ToString("MMM dd, HH:mm:ss");
            }

            return dateTime.ToString("MMM dd yyyy, HH:mm:ss");
        }

        /// <summary>
        /// Returns a string representation of the number of days (when less than 7) or number of weeks between the first and last day.
        /// </summary>
        public static string DaysOrWeeksUntil(this DateOnly firstDay, DateOnly lastDay, string day = "day", string days = "days", string week = "week", string weeks = "weeks")
        {
            if (firstDay > lastDay)
            {
                throw new ArgumentException("First day must be before last day");
            }

            var numDays = (lastDay.DayNumber - firstDay.DayNumber) + 1;
            var numWeeks = numDays >= 7 ? (int)Math.Ceiling(numDays / 7f) : 0;

            return numWeeks switch
            {
                0 => numDays == 1
                    ? "1 " + day
                    : numDays + " " + days,
                1 => "1 " + week,
                _ => numWeeks + " " + weeks
            };
        }

        /// <summary>
        /// Returns a human-readable string representation of the number of calendar weeks between the first and last day.
        ///
        /// Examples: "Week 15 2023", "Week 1 until 5 2023", "Week 52 2022 until 5 2023".
        /// </summary>
        public static string WeekNumbersUntil(this DateOnly firstDay, DateOnly lastDay, string week = "Week", string until = "until")
        {
            if (lastDay < firstDay)
            {
                throw new ArgumentException("First day must be before last day");
            }

            var startWeek = ISOWeek.GetWeekOfYear(firstDay.ToDateTime(default));
            var endWeek = ISOWeek.GetWeekOfYear(lastDay.ToDateTime(default));
            
            if (firstDay.Year == lastDay.Year && startWeek == endWeek)
            {
                return week + " " + startWeek + " " + lastDay.Year;
            }

            if (firstDay.Year != lastDay.Year)
            {
                return week + " " + startWeek + " " + firstDay.Year +
                    " " + until + " "
                    + endWeek + " " + lastDay.Year;
            }

            return week + " " + startWeek + " " + until + " " + endWeek + " " + lastDay.Year;
        }

        /// <summary>
        /// Returns the start date of the week in which the given <paramref name="dateTime"/> falls, provided the week starts at the weekday <paramref name="startOfWeek"/>.
        /// </summary>
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dateTime.DayOfWeek - startOfWeek)) % 7;
            return dateTime.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Get occurrences per <see cref="DayOfWeek"/> between <paramref name="from"/> and <paramref name="until"/>.
        /// </summary>
        public static Dictionary<DayOfWeek, int> GetDayFrequency(this DateTime from, DateTime until)
        {
            var startDay = DateOnly.FromDateTime(from);
            var endDay = DateOnly.FromDateTime(until);

            return startDay.GetDayFrequency(endDay);
        }

        /// <summary>
        /// Get occurrences per <see cref="DayOfWeek"/> between <paramref name="from"/> and <paramref name="until"/>.
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
