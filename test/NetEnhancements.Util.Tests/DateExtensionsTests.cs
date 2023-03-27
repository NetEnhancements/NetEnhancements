namespace NetEnhancements.Util.Tests
{
    public class DateExtensionsTests
    {
        [Test]
        public void RoundToNearest_Goes_Up()
        {
            // Arrange
            var dt = new DateTime(2019, 5, 11, 8, 0, 35);
            var expected = new DateTime(2019, 5, 11, 8, 1, 0);

            // Act and Assert
            Assert.AreEqual(expected, dt.RoundToNearest(TimeSpan.FromMinutes(1)));
        }

        [Test]
        public void RoundToNearest_Increments_Hours()
        {
            // Arrange
            var dt = new DateTime(2019, 5, 11, 7, 59, 35);
            var expected = new DateTime(2019, 5, 11, 8, 0, 0);

            // Act and Assert
            Assert.AreEqual(expected, dt.RoundToNearest(TimeSpan.FromMinutes(1)));
        }

        [Test]
        public void IsBetweenTwoDates_DateTimeIsBetween_ReturnsTrue()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1);
            var end = new DateTime(2022, 12, 31);
            var dt = new DateTime(2022, 6, 1);

            // Act
            var result = dt.IsBetweenTwoDates(start, end);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsBetweenTwoDates_DateTimeIsBeforeStart_ReturnsFalse()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1);
            var end = new DateTime(2022, 12, 31);
            var dt = new DateTime(2021, 12, 31);

            // Act
            var result = dt.IsBetweenTwoDates(start, end);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsBetweenTwoDates_DateTimeIsAfterEnd_ReturnsFalse()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1);
            var end = new DateTime(2022, 12, 31);
            var dt = new DateTime(2023, 1, 1);

            // Act
            var result = dt.IsBetweenTwoDates(start, end);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Humanize_ZeroTimeSpan_ReturnsEmptyString()
        {
            // Arrange
            var ts = TimeSpan.Zero;

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("0:00:00", result);
        }

        [Test]
        public void Humanize_OneDayTimeSpan_ReturnsFormattedString()
        {
            // Arrange
            var ts = new TimeSpan(1, 0, 0, 0);

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("1d", result);
        }

        [Test]
        public void Humanize_OneHourTimeSpan_ReturnsFormattedString()
        {
            // Arrange
            var ts = new TimeSpan(0, 1, 0, 0);

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("1u", result);
        }

        [Test]
        public void Humanize_OneMinuteTimeSpan_ReturnsFormattedString()
        {
            // Arrange
            var ts = new TimeSpan(0, 0, 1, 0);

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("1m", result);
        }

        [Test]
        public void Humanize_OneSecondTimeSpan_ReturnsFormattedString()
        {
            // Arrange
            var ts = new TimeSpan(0, 0, 0, 1);

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("1s", result);
        }

        [Test]
        public void Humanize_OneMillisecondTimeSpan_ReturnsFormattedString()
        {
            // Arrange
            var ts = new TimeSpan(0, 0, 0, 0, 1);

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("01ms", result);
        }

        [Test]
        public void Humanize_MultipleTimeUnits_ReturnsFormattedString()
        {
            // Arrange
            var ts = new TimeSpan(1, 2, 3, 4, 5);

            // Act
            var result = ts.Humanize();

            // Assert
            Assert.AreEqual("1d 2u 3m 4s 5ms", result);
        }

        [Test]
        public void GetFirstDayOfMonth_ReturnsFirstMonday_WhenCalledWithMonday()
        {
            // Arrange
            var date = new DateTime(2023, 3, 19); // a Sunday
            var firstDayOfWeek = DayOfWeek.Monday;
            var expectedFirstDay = new DateTime(2023, 3, 6); // the first Monday in March 2023

            // Act
            var result = date.GetFirstDayOfMonth(firstDayOfWeek);

            // Assert
            Assert.AreEqual(expectedFirstDay, result);
        }

        [Test]
        public void GetFirstDayOfMonth_ReturnsFirstSunday_WhenCalledWithSunday()
        {
            // Arrange
            var date = new DateTime(2023, 3, 19); // a Sunday
            var firstDayOfWeek = DayOfWeek.Sunday;
            var expectedFirstDay = new DateTime(2023, 3, 5); // the first Sunday in March 2023

            // Act
            var result = date.GetFirstDayOfMonth(firstDayOfWeek);

            // Assert
            Assert.AreEqual(expectedFirstDay, result);
        }

        [Test]
        public void GetFirstDayOfMonth_ReturnsFirstTuesday_WhenCalledWithTuesday()
        {
            // Arrange
            var date = new DateTime(2023, 3, 19); // a Sunday
            var firstDayOfWeek = DayOfWeek.Tuesday;
            var expectedFirstDay = new DateTime(2023, 3, 7); // the first Tuesday in March 2023

            // Act
            var result = date.GetFirstDayOfMonth(firstDayOfWeek);

            // Assert
            Assert.AreEqual(expectedFirstDay, result);
        }

        [Test]
        public void DoDatesOverlap_WhenAEndIsBeforeBStart_ReturnsFalse()
        {
            // Arrange
            var aStart = new DateTime(2023, 3, 1);
            var aEnd = new DateTime(2023, 3, 10);
            var bStart = new DateTime(2023, 3, 15);
            var bEnd = new DateTime(2023, 3, 20);

            // Act
            var result = DateExtensions.DoDatesOverlap(aStart, aEnd, bStart, bEnd);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void DoDatesOverlap_WhenBEndIsBeforeAStart_ReturnsFalse()
        {
            // Arrange
            var aStart = new DateTime(2023, 3, 15);
            var aEnd = new DateTime(2023, 3, 20);
            var bStart = new DateTime(2023, 3, 1);
            var bEnd = new DateTime(2023, 3, 10);

            // Act
            var result = DateExtensions.DoDatesOverlap(aStart, aEnd, bStart, bEnd);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void DoDatesOverlap_WhenADateRangeIsContainedInBDateRange_ReturnsTrue()
        {
            // Arrange
            var aStart = new DateTime(2023, 3, 5);
            var aEnd = new DateTime(2023, 3, 10);
            var bStart = new DateTime(2023, 3, 1);
            var bEnd = new DateTime(2023, 3, 15);

            // Act
            var result = DateExtensions.DoDatesOverlap(aStart, aEnd, bStart, bEnd);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DoDatesOverlap_WhenBDateRangeIsContainedInADateRange_ReturnsTrue()
        {
            // Arrange
            var aStart = new DateTime(2023, 3, 1);
            var aEnd = new DateTime(2023, 3, 15);
            var bStart = new DateTime(2023, 3, 5);
            var bEnd = new DateTime(2023, 3, 10);

            // Act
            var result = DateExtensions.DoDatesOverlap(aStart, aEnd, bStart, bEnd);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DoDatesOverlap_WhenDateRangesOverlap_ReturnsTrue()
        {
            // Arrange
            var aStart = new DateTime(2023, 3, 5);
            var aEnd = new DateTime(2023, 3, 15);
            var bStart = new DateTime(2023, 3, 10);
            var bEnd = new DateTime(2023, 3, 20);

            // Act
            var result = DateExtensions.DoDatesOverlap(aStart, aEnd, bStart, bEnd);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void RoundToNearest_RoundsToNearestDay()
        {
            // Arrange
            var input = new TimeSpan(0, 23, 59, 59, 999);
            var roundTo = new TimeSpan(1, 0, 0, 0);

            // Act
            var result = input.RoundToNearest(roundTo);

            // Assert
            var expected = new TimeSpan(1, 0, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RoundToNearest_RoundsToNearestHour()
        {
            // Arrange
            var input = new TimeSpan(0, 59, 59);
            var roundTo = new TimeSpan(0, 1, 0, 0);

            // Act
            var result = input.RoundToNearest(roundTo);

            // Assert
            var expected = new TimeSpan(1, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RoundToNearest_RoundsToNearestMinute()
        {
            // Arrange
            var input = new TimeSpan(0, 0, 29, 59);
            var roundTo = new TimeSpan(0, 0, 1, 0);

            // Act
            var result = input.RoundToNearest(roundTo);

            // Assert
            var expected = new TimeSpan(0, 0, 30, 0);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RoundToNearest_RoundsToNearestSecond()
        {
            // Arrange
            var input = new TimeSpan(0, 0, 0, 29, 500);
            var roundTo = new TimeSpan(0, 0, 0, 1);

            // Act
            var result = input.RoundToNearest(roundTo);

            // Assert
            var expected = new TimeSpan(0, 0, 30);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RoundToNearest_RoundsToNearestSecond_WithNoMilliseconds()
        {
            // Arrange
            var input = new TimeSpan(0, 0, 0, 29, 999);
            var roundTo = new TimeSpan(0, 0, 0, 1);

            // Act
            var result = input.RoundToNearest(roundTo);

            // Assert
            var expected = new TimeSpan(0, 0, 30);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void EachDay_SameDay_ReturnsOneDay()
        {
            // Arrange
            var from = new DateTime(2023, 3, 24);
            var thru = new DateTime(2023, 3, 24);

            // Act
            var result = DateExtensions.EachDay(from, thru).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(from));
        }

        [Test]
        public void EachDay_OneDayApart_ReturnsTwoDays()
        {
            // Arrange
            var from = new DateTime(2023, 3, 24);
            var thru = new DateTime(2023, 3, 25);

            // Act
            var result = DateExtensions.EachDay(from, thru).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(from));
            Assert.That(result[1], Is.EqualTo(thru));
        }

        [Test]
        public void EachDay_MultipleDays_ReturnsExpectedDays()
        {
            // Arrange
            var from = new DateTime(2023, 3, 24);
            var thru = new DateTime(2023, 3, 27);

            // Act
            var result = DateExtensions.EachDay(from, thru).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0], Is.EqualTo(new DateTime(2023, 3, 24)));
            Assert.That(result[1], Is.EqualTo(new DateTime(2023, 3, 25)));
            Assert.That(result[2], Is.EqualTo(new DateTime(2023, 3, 26)));
            Assert.That(result[3], Is.EqualTo(new DateTime(2023, 3, 27)));
        }

        [Test]
        public void EachDay_SameDay_ReturnsZeroDay()
        {
            // Arrange
            var from = new DateTime(2023, 3, 25);
            var thru = new DateTime(2023, 3, 24);

            // Act
            var result = DateExtensions.EachDay(from, thru).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestFromUnixTime_Epoch()
        {
            // Arrange
            long unixTime = 0;
            DateTime expected = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

            // Act
            DateTime actual = DateExtensions.FromUnixTime(unixTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFromUnixTime_Positive()
        {
            // Arrange
            long unixTime = 1679170800;
            DateTime expected = new DateTime(2023, 3, 18, 20, 20, 0, DateTimeKind.Utc).ToLocalTime();

            // Act
            DateTime actual = DateExtensions.FromUnixTime(unixTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFromUnixTime_Negative()
        {
            // Arrange
            long unixTime = -2208988800; 
            DateTime expected = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            
            // Act
            DateTime actual = DateExtensions.FromUnixTime(unixTime);
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestToUnixTime_MinValue()
        {
            // Arrange
            DateTime dt = DateTime.MinValue;
            long expected = 0;
            
            // Act
            long actual = dt.ToUnixTime();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestToUnixTime_Positive()
        {
            // Arrange
            DateTime dt = new DateTime(2023, 3, 18, 20, 20, 0, DateTimeKind.Utc);
            long expected = 1679170800;

            // Act
            long actual = dt.ToUnixTime();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestToUnixTime_Negative()
        {
            // Arrange
            DateTime dt = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long expected = -2208988800; // 1900-01-01 00:00:00 UTC
            
            // Act
            long actual = dt.ToUnixTime();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStartOfPreviousMonth_SameYear()
        {
            // Arrange
            DateTime dt = new DateTime(2023, 5, 15);
            DateTime expected = new DateTime(2023, 4, 1);

            // Act
            DateTime actual = dt.StartOfPreviousMonth();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStartOfPreviousMonth_ChangeYear()
        {
            // Arrange
            DateTime dt = new DateTime(2023, 1, 10);
            DateTime expected = new DateTime(2022, 12, 1);

            // Act
            DateTime actual = dt.StartOfPreviousMonth();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStartOfPreviousMonth_LeapYear()
        {
            // Arrange
            DateTime dt = new DateTime(2020, 3, 1);
            DateTime expected = new DateTime(2020, 2, 1);

            // Act
            DateTime actual = dt.StartOfPreviousMonth();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestEndOfPreviousMonth_SameYear()
        {
            // Arrange
            DateTime dt = new DateTime(2021, 5, 15);
            DateTime expected = new DateTime(2021, 4, 30, 23, 59, 59, 999, DateTimeKind.Local);

            // Act
            DateTime actual = dt.EndOfPreviousMonth();

            // Assert
            Assert.AreEqual(expected.Date, actual.Date);
        }

        [Test]
        public void TestEndOfPreviousMonth_ChangeYear()
        {
            // Arrange
            DateTime dt = new DateTime(2021, 1, 10);
            DateTime expected = new DateTime(2020, 12, 31, 23, 59, 59, 999, DateTimeKind.Local);

            // Act
            DateTime actual = dt.EndOfPreviousMonth();

            // Assert
            Assert.AreEqual(expected.Date, actual.Date);
        }

        [Test]
        public void TestEndOfPreviousMonth_LeapYear()
        {
            // Arrange
            DateTime dt = new DateTime(2020, 3, 1);
            DateTime expected = new DateTime(2020, 2, 29, 23, 59, 59, 999, DateTimeKind.Local);

            // Act
            DateTime actual = dt.EndOfPreviousMonth();

            // Assert
            Assert.AreEqual(expected.Date, actual.Date);
        }

        [Test]
        public void StartOfWeek_Goes_Back()
        {
            // Arrange Monday
            var date = new DateTime(2023, 3, 27);

            // Act
            var weekStart = date.StartOfWeek(DayOfWeek.Sunday);

            // Assert Sunday
            Assert.That(weekStart, Is.EqualTo(new DateTime(2023, 3, 26)));
        }

        [Test]
        public void StartOfWeek_Keeps_WeekStart()
        {
            // Arrange Monday
            var date = new DateTime(2023, 3, 27);

            // Act
            var weekStart = date.StartOfWeek(DayOfWeek.Monday);

            // Assert still Monday
            Assert.That(weekStart, Is.EqualTo(date));
        }

        [Test]
        [TestCaseSource(nameof(ToReadableString_DataSource))]
        public void ToReadableString((DateTimeOffset? DateTimeOffset, bool UseValue, string? Expected) testData)
        {
            var (dateTimeOffset, useValue, expected) = testData;
            
            // UseValue calls using .Value, the non-nullable overload.
            Assert.That(useValue ? dateTimeOffset!.Value.ToReadableString() : dateTimeOffset.ToReadableString(), Is.EqualTo(expected));
        }

        private static List<(DateTimeOffset? DateTimeOffset, bool UseValue, string? Expected)> ToReadableString_DataSource()
        {
            var now = DateTimeOffset.Now;

            var nineMonthsAgo = now.AddMonths(-9).Date.AddHours(5).AddMinutes(6).AddSeconds(7);
            var elevenMonthsAgo = now.AddMonths(-11).Date.AddHours(6).AddMinutes(7).AddSeconds(8);

            return new List<(DateTimeOffset? DateTimeOffset, bool UseValue, string? Expected)>
            {
                // Nullable overload
                (null, false, null),
                (new DateTimeOffset(now.Date.AddHours(9).AddMinutes(12).AddSeconds(15), TimeSpan.FromHours(2)), false, "today, 09:12:15"),

                // Non-nullable overload
                (new DateTimeOffset(now.Date.AddHours(9).AddMinutes(12).AddSeconds(15), TimeSpan.FromHours(2)), true, "today, 09:12:15"),
                (new DateTimeOffset(now.Date.AddDays(-1).AddHours(19).AddMinutes(12).AddSeconds(15), TimeSpan.FromHours(2)), true, "yesterday, 19:12:15"),
                (new DateTimeOffset(now.Date.AddDays(-1).AddHours(19).AddMinutes(12).AddSeconds(15), TimeSpan.FromHours(2)), true, "yesterday, 19:12:15"),
                
                (new DateTimeOffset(nineMonthsAgo, TimeSpan.FromHours(2)), true, nineMonthsAgo.ToString("MMM dd, HH:mm:ss")),
                (new DateTimeOffset(elevenMonthsAgo, TimeSpan.FromHours(2)), true, elevenMonthsAgo.ToString("MMM dd yyyy, HH:mm:ss")),
            };
        }
    }
}
