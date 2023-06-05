namespace NetEnhancements.Util.Tests
{
    public class WeekTests
    {
        /// <summary>
        /// Wednesday till Wednesday two weeks later = 3 weeks
        /// </summary>
        [Test]
        public void WeeksBeteen_Midweek_AddsWeek()
        {
            // Arrange
            var startDate = new DateOnly(2023, 05, 11);
            var endDate = new DateOnly(2023, 05, 25);

            // Act
            var weeks = Week.GetBetween(startDate, endDate);

            // Assert
            Assert.That(weeks, Has.Count.EqualTo(3));
            Assert.Multiple(() =>
            {
                Assert.That(weeks[0].Start, Is.EqualTo(startDate));
                Assert.That(weeks[0].End, Is.EqualTo(new DateOnly(2023, 05, 14)));
                Assert.That(weeks[1].Start, Is.EqualTo(new DateOnly(2023, 05, 15)));
                Assert.That(weeks[1].End, Is.EqualTo(new DateOnly(2023, 05, 21)));
                Assert.That(weeks[2].Start, Is.EqualTo(new DateOnly(2023, 05, 22)));
                Assert.That(weeks[2].End, Is.EqualTo(endDate));
            });
        }

        [Test]
        public void WeeksBeteen_FullWeek_OneWeek()
        {
            // Arrange
            var startDate = new DateOnly(2023, 05, 29);
            var endDate = new DateOnly(2023, 06, 04);

            // Act
            var weeks = Week.GetBetween(startDate, endDate);

            // Assert
            Assert.That(weeks, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(weeks.First().Start, Is.EqualTo(startDate));
                Assert.That(weeks.Last().End, Is.EqualTo(endDate));
            });
        }

        [Test]
        public void WeeksBeteen_SubWeek_OneWeek()
        {
            // Arrange
            var startDate = new DateOnly(2023, 05, 04);
            var endDate = new DateOnly(2023, 05, 05);

            // Act
            var weeks = Week.GetBetween(startDate, endDate);

            // Assert
            Assert.That(weeks, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(weeks.First().Start, Is.EqualTo(startDate));
                Assert.That(weeks.Last().End, Is.EqualTo(endDate));
            });
        }

        [Test]
        public void WeeksBeteen_OneDay_OneWeek()
        {
            // Arrange
            var startDate = new DateOnly(2023, 05, 04);
            var endDate = new DateOnly(2023, 05, 04);

            // Act
            var weeks = Week.GetBetween(startDate, endDate);

            // Assert
            Assert.That(weeks, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(weeks.First().Start, Is.EqualTo(startDate));
                Assert.That(weeks.Last().End, Is.EqualTo(endDate));
            });
        }
    }
}
