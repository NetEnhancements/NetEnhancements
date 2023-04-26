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
        /// <example>
        /// <code>
        /// DateTime dt = new DateTime(2023, 4, 10, 14, 27, 36);
        /// TimeSpan ts = TimeSpan.FromMinutes(15);
        /// DateTime rounded = dt.RoundToNearest(ts);
        /// Console.WriteLine($"Rounded time: {rounded}");
        /// // Output: Rounded time: 4/10/2023 2:30:00 PM
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// TimeSpan ts = TimeSpan.FromMinutes(75);
        /// TimeSpan rounded = ts.RoundToNearest(TimeSpan.FromMinutes(30));
        /// Console.WriteLine($"Rounded time: {rounded}");
        /// // Output: Rounded time: 1:30:00
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// DateTime now = DateTime.Now;
        /// DateTime start = new DateTime(2022, 1, 1);
        /// DateTime end = new DateTime(2022, 12, 31);
        /// bool isBetween = now.IsBetweenTwoDates(start, end);
        /// Console.WriteLine($"Is {now} between {start} and {end}? {isBetween}");
        /// // Output: Is {now} between {start} and {end}? True or False
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// DateTime aStart = new DateTime(2022, 01, 01);
        /// DateTime aEnd = new DateTime(2022, 01, 15);
        /// DateTime bStart = new DateTime(2022, 01, 10);
        /// DateTime bEnd = new DateTime(2022, 01, 20);
        /// bool overlap = DoDatesOverlap(aStart, aEnd, bStart, bEnd);
        /// Console.WriteLine($"Do the date ranges [{aStart:dd/MM/yyyy} - {aEnd:dd/MM/yyyy}] and [{bStart:dd/MM/yyyy} - {bEnd:dd/MM/yyyy}] overlap? {overlap}");
        /// // Output: Do the date ranges [01/01/2022 - 15/01/2022] and [10/01/2022 - 20/01/2022] overlap? True
        /// </code>
        /// </example>
        public static bool DoDatesOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
        {
            return aStart < bEnd && bStart < aEnd;
        }

        /// <summary>
        /// Converts a Unix time to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The Unix time to convert, in seconds since the Unix epoch (1970-01-01 00:00:00 UTC).</param>
        /// <returns>A DateTime object representing the converted Unix time.</returns>
        /// <example>
        /// <code>
        /// long unixTime = 1649714568;
        /// DateTime dt = FromUnixTime(unixTime);
        /// Console.WriteLine($"The Unix time {unixTime} corresponds to {dt.ToString()}.");
        /// // Output: The Unix time 1649714568 corresponds to 2022-04-10 09:09:28 AM.
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// DateTime dateTime = new DateTime(2022, 4, 11, 13, 45, 0);
        /// long unixTime = dateTime.ToUnixTime();
        /// Console.WriteLine($"The Unix time of {dateTime} is {unixTime}.");
        /// // Output: The Unix time of 04/11/2022 13:45:00 is 1649684700.
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// var startDate = new DateTime(2023, 04, 01);
        /// var endDate = new DateTime(2023, 04, 10);
        /// var days = DateTimeExtensions.EachDay(startDate, endDate);
        /// foreach (var day in days)
        /// {
        ///     Console.WriteLine(day.ToString("yyyy-MM-dd"));
        /// }
        /// 
        /// // Output:
        /// // 2023-04-01
        /// // 2023-04-02
        /// // 2023-04-03
        /// // 2023-04-04
        /// // 2023-04-05
        /// // 2023-04-06
        /// // 2023-04-07
        /// // 2023-04-08
        /// // 2023-04-09
        /// // 2023-04-10
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// DateTime dt = DateTime(2022, 4, 11, 13, 45, 0);
        /// DateTime startOfPrevMonth = dt.StartOfPreviousMonth();
        /// Console.WriteLine($"The start of the previous month from {dt} is {startOfPrevMonth}");
        /// // Output: The start of the previous month from 04/11/2022 13:45:00 is 03/01/2022 00:00:00
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// DateTime today = DateTime.Now;
        /// DateTime endOfPreviousMonth = today.EndOfPreviousMonth();
        /// Console.WriteLine($"The end of the previous month from {today:D} is {endOfPreviousMonth:D}");
        /// // Output: The end of the previous month from Sunday, April 11, 2023 is Sunday, February 28, 2023
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2023, 4, 1);
        /// DayOfWeek firstDayOfWeek = DayOfWeek.Monday;
        /// DateTime firstMondayOfMonth = date.GetFirstDayOfMonth(firstDayOfWeek);
        /// Console.WriteLine(firstMondayOfMonth);
        /// // Output: 04/03/2023 12:00:00 AM
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// TimeSpan duration = TimeSpan.FromSeconds(180);
        /// string humanizedDuration = duration.Humanize();
        /// Console.WriteLine(humanizedDuration);
        /// // Output: 3m
        /// </code>
        /// </example>
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
        /// <param name="dateTime">The DateTimeOffset to represent as a string.</param>
        /// <param name="today">The string to use to represent "today". Defaults to "today".</param>
        /// <param name="yesterday">The string to use to represent "yesterday". Defaults to "yesterday".</param>
        /// <returns>A string representing the given DateTimeOffset in a human-readable format.</returns>
        /// <example>
        /// <code>
        /// DateTimeOffset dt1 = DateTimeOffset.Now;
        /// DateTimeOffset dt2 = DateTimeOffset.Now.AddDays(-1);
        /// DateTimeOffset dt3 = DateTimeOffset.Now.AddDays(-2);
        ///
        /// Console.WriteLine(dt1.ToReadableString()); // Output: "today, 14:30:45"
        /// Console.WriteLine(dt2.ToReadableString()); // Output: "yesterday, 14:30:45"
        /// Console.WriteLine(dt3.ToReadableString()); // Output: "Apr 08, 14:30:45"
        /// </code>
        /// </example>
        public static string? ToReadableString(this DateTimeOffset? dateTime, string today = "today", string yesterday = "yesterday") => dateTime?.ToReadableString(today, yesterday);

        /// <summary>
        /// Readable date string: "today, HH:mm:ss", "yesterday, HH:mm:ss", "Jan 01, HH:mm:ss", ...
        /// </summary>
        /// <param name="dateTime">The DateTimeOffset object to convert.</param>
        /// <param name="today">The string to use for the current day.</param>
        /// <param name="yesterday">The string to use for the previous day.</param>
        /// <returns>A human-readable string representing the DateTimeOffset object.</returns>
        /// <example>
        /// <code>
        /// DateTimeOffset dateTime = DateTimeOffset.Now;
        /// string today = "today";
        /// string yesterday = "yesterday";
        /// string readableDateTime = dateTime.ToReadableString(today, yesterday);
        /// Console.WriteLine(readableDateTime);
        /// // Output: "today, 12:34:56"
        /// </code>
        /// </example>
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
        /// <param name="firstDay">The first day in the range.</param>
        /// <param name="lastDay">The last day in the range.</param>
        /// <param name="day">The singular name of a day.</param>
        /// <param name="days">The plural name of days.</param>
        /// <param name="week">The singular name of a week.</param>
        /// <param name="weeks">The plural name of weeks.</param>
        /// <returns>A string representation of the number of days or weeks between the two dates.</returns>
        /// <example>
        /// <code>
        /// DateOnly firstDay = new DateOnly(2023, 4, 1);
        /// DateOnly lastDay = new DateOnly(2023, 4, 11);
        /// string result = firstDay.DaysOrWeeksUntil(lastDay);
        /// Console.WriteLine(result);
        /// // Output: 11 days
        /// </code>
        /// </example>
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
        /// <param name="firstDay">The first day of the period to calculate the week numbers for.</param>
        /// <param name="lastDay">The last day of the period to calculate the week numbers for.</param>
        /// <param name="week">The text to use for the word "Week" in the output string.</param>
        /// <param name="until">The text to use for the word "until" in the output string.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="lastDay"/> is before <paramref name="firstDay"/>.</exception>
        /// <returns>A human-readable string representation of the number of calendar weeks between the first and last day.</returns>
        /// <example>
        /// <code>
        /// var firstDay = new DateOnly(2023, 3, 27);
        /// var lastDay = new DateOnly(2023, 4, 30);
        /// var weekNumbers = firstDay.WeekNumbersUntil(lastDay);
        /// Console.WriteLine($"Week numbers: {weekNumbers}");
        /// // Output: Week 13 until 17 2023
        /// </code>
        /// </example>
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
        /// <param name="dateTime">The date for which to calculate the start of the week.</param>
        /// <param name="startOfWeek">The day of the week that starts the week.</param>
        /// <returns>The start date of the week in which <paramref name="dateTime"/> falls.</returns>
        /// <example>
        /// This example returns the start date of the week containing the given date, assuming Monday starts the week:
        /// <code>
        /// DateTime date = new DateTime(2023, 4, 10);
        /// DateTime startOfWeek = date.StartOfWeek(DayOfWeek.Monday); // Returns 2023-04-10 (Monday)
        /// Console.WriteLine($"Start of the week: {startOfWeek}");
        /// // Output: 2023-04-10
        /// </code>
        /// </example>
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dateTime.DayOfWeek - startOfWeek)) % 7;
            return dateTime.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Get occurrences per <see cref="DayOfWeek"/> between <paramref name="from"/> and <paramref name="until"/>.
        /// </summary>
        /// <param name="from">The start date for the calculation.</param>
        /// <param name="until">The end date for the calculation.</param>
        /// <returns>A dictionary containing the frequency of occurrences for each day of the week.</returns>
        /// <example>
        /// <code>
        /// var from = new DateTime(2023, 1, 1);
        /// var until = new DateTime(2023, 1, 31);
        /// var result = DateTimeExtensions.GetDayFrequency(from, until);
        /// Console.WriteLine("Occurrences per day of the week:");
        /// foreach (var item in result)
        /// {
        ///     Console.WriteLine($"{item.Key}: {item.Value}");
        /// }
        /// // Output: Occurrences per day of the week:
        /// // Output: Sunday: 5
        /// // Output: Monday: 5
        /// // Output: Tuesday: 5
        /// // Output: Wednesday: 4
        /// // Output: Thursday: 4
        /// // Output: Friday: 4
        /// // Output: Saturday: 4
        /// </code>
        /// </example>
        public static Dictionary<DayOfWeek, int> GetDayFrequency(this DateTime from, DateTime until)
        {
            var startDay = DateOnly.FromDateTime(from);
            var endDay = DateOnly.FromDateTime(until);

            return startDay.GetDayFrequency(endDay);
        }

        /// <summary>
        /// Get occurrences per <see cref="DayOfWeek"/> between <paramref name="from"/> and <paramref name="until"/>.
        /// </summary>
        /// <param name="from">The starting date of the range to count.</param>
        /// <param name="until">The ending date of the range to count (inclusive).</param>
        /// <returns>A dictionary containing the number of occurrences of each <see cref="DayOfWeek"/> in the range.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="until"/> is before <paramref name="from"/>.</exception>
        /// <example>
        /// <code>
        /// var from = new DateOnly(2022, 1, 1);
        /// var until = new DateOnly(2022, 12, 31);
        /// var dayFrequency = DateTimeExtensions.GetDayFrequency(from, until);
        ///
        /// foreach (var kvp in dayFrequency)
        /// {
        ///     Console.WriteLine($"{kvp.Key}: {kvp.Value} occurrences");
        /// }
        /// // Output:Monday: 52 occurrences
        /// // Output:Tuesday: 52 occurrences
        /// // Output:Wednesday: 52 occurrences
        /// // Output:Thursday: 52 occurrences
        /// // Output:Friday: 52 occurrences
        /// // Output:Saturday: 52 occurrences
        /// // Output:Sunday: 52 occurrences
        /// </code>
        /// </example>
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
