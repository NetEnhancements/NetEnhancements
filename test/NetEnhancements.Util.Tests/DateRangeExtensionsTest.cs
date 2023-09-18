namespace NetEnhancements.Util.Tests
{
    public class DateRangeExtensionsTests
    {
        [Test]
        public void GetDayFrequency_Throws()
        {
            // Arrange
            var from = new DateTime(2023, 04, 02);
            var until = from.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => from.GetDayFrequency(until));
        }

        [Test]
        public void GetDayFrequency_Counts_DateTimes()
        {
            // Arrange
            var from = new DateTime(2023, 04, 02);
            var until = new DateTime(2023, 04, 04);

            // Act
            var frequencies = from.GetDayFrequency(until);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(frequencies, Has.Count.EqualTo(7));

                Assert.That(frequencies[DayOfWeek.Sunday], Is.EqualTo(1));
                Assert.That(frequencies[DayOfWeek.Monday], Is.EqualTo(1));
                Assert.That(frequencies[DayOfWeek.Tuesday], Is.EqualTo(1));
                Assert.That(frequencies[DayOfWeek.Wednesday], Is.EqualTo(0));
                Assert.That(frequencies[DayOfWeek.Thursday], Is.EqualTo(0));
                Assert.That(frequencies[DayOfWeek.Friday], Is.EqualTo(0));
                Assert.That(frequencies[DayOfWeek.Saturday], Is.EqualTo(0));
            });
        }

        [Test]
        public void GetDayFrequency_Counts_DateOnlys()
        {
            // Arrange
            var from = new DateOnly(2023, 03, 26);
            var until = new DateOnly(2023, 04, 04);

            // Act
            var frequencies = from.GetDayFrequency(until);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(frequencies, Has.Count.EqualTo(7));

                Assert.That(frequencies[DayOfWeek.Sunday], Is.EqualTo(2));
                Assert.That(frequencies[DayOfWeek.Monday], Is.EqualTo(2));
                Assert.That(frequencies[DayOfWeek.Tuesday], Is.EqualTo(2));
                Assert.That(frequencies[DayOfWeek.Wednesday], Is.EqualTo(1));
                Assert.That(frequencies[DayOfWeek.Thursday], Is.EqualTo(1));
                Assert.That(frequencies[DayOfWeek.Friday], Is.EqualTo(1));
                Assert.That(frequencies[DayOfWeek.Saturday], Is.EqualTo(1));
            });
        }

        [Test]
        public void DaysOrWeeksUntil_Throws()
        {
            var start = new DateOnly(2023, 04, 26);
            var end = start.AddDays(-1);

            Assert.Throws<ArgumentException>(() => start.DaysOrWeeksUntil(end));
        }

        [Test]
        public void DaysOrWeeksUntil_1Day()
        {
            // Arrange
            var start = new DateOnly(2023, 04, 26);
            var end = start;

            // Act
            var text = start.DaysOrWeeksUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("1 day"));
        }

        [Test]
        public void DaysOrWeeksUntil_3Days()
        {
            // Arrange
            var start = new DateOnly(2023, 04, 26);
            var end = start.AddDays(2);

            // Act
            var text = start.DaysOrWeeksUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("3 days"));
        }

        [Test]
        public void DaysOrWeeksUntil_1Week()
        {
            // Arrange
            var start = new DateOnly(2023, 04, 26);
            var end = start.AddDays(6);

            // Act
            var text = start.DaysOrWeeksUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("1 week"));
        }

        [Test]
        public void DaysOrWeeksUntil_3Weeks()
        {
            // Arrange
            var start = new DateOnly(2023, 04, 26);
            var end = start.AddDays(20);

            // Act
            var text = start.DaysOrWeeksUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("3 weeks"));
        }

        [Test]
        public void WeekNumbersUntil_Throws()
        {
            var start = new DateOnly(2023, 04, 26);
            var end = start.AddDays(-1);

            Assert.Throws<ArgumentException>(() => start.WeekNumbersUntil(end));
        }

        [Test]
        public void WeekNumbersUntil_1Week()
        {
            // Arrange
            var start = new DateOnly(2023, 04, 26);
            var end = start;

            // Act
            var text = start.WeekNumbersUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("Week 17 2023"));
        }

        [Test]
        public void WeekNumbersUntil_3Weeks()
        {
            // Arrange
            var start = new DateOnly(2023, 02, 15);
            var end = new DateOnly(2023, 03, 17);

            // Act
            var text = start.WeekNumbersUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("Week 7 until 11 2023"));
        }

        [Test]
        public void WeekNumbersUntil_YearBoundary()
        {
            // Arrange
            var start = new DateOnly(2022, 12, 31);
            var end = new DateOnly(2023, 01, 31);

            // Act
            var text = start.WeekNumbersUntil(end);

            // Assert
            Assert.That(text, Is.EqualTo("Week 52 2022 until 5 2023"));
        }
    }
}
