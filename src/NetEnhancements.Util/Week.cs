using System.Globalization;

namespace NetEnhancements.Util;

/// <summary>
/// Represents a calendar week.
/// </summary>
public sealed record Week(DateOnly Start, DateOnly End)
{
    /// <summary>
    /// The ISO 8601 week number.
    /// </summary>
    public int Number => ISOWeek.GetWeekOfYear(Start.ToDateTime(default));

    /// <summary>
    /// Get the weeks between two dates.
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="firstDayOfWeek"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">When the end date is before the start date.</exception>
    public static IReadOnlyList<Week> GetBetween(DateOnly startDate, DateOnly endDate, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("endDate must lie on or beyond startDate");
        }

        if (endDate == startDate) return new[]
        {
            new Week(startDate, endDate)
        };

        var lastDayOfWeek = firstDayOfWeek - 1 + 7 % 7;

        var weeks = new List<Week>();
        var weekEndDate = new[] { endDate, startDate.Next(lastDayOfWeek) }.Min();

        for (
            var weekStartDate = startDate;
            weekStartDate < endDate;
            weekStartDate = weekEndDate.AddDays(1),
            weekEndDate = new[] { endDate, weekEndDate.AddDays(7) }.Min()
        )
        {
            weeks.Add(new Week(weekStartDate, weekEndDate));
        }

        return weeks.ToArray();
    }
}
