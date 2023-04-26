namespace NetEnhancements.EntityFramework.Tests
{
    public class DateOnlyConverterTests
    {
        private readonly DateTime _dateTime = new(2023, 03, 27);
        private readonly DateOnly _dateOnly = new(2023, 03, 27);

        [Test]
        public void DateOnlyConverter_From()
        {
            // Arrange
            var converter = new DateOnlyConverter();

            // Act
            var output = converter.ConvertFromProviderExpression.Compile().Invoke(_dateTime);

            // Assert
            Assert.That(output, Is.EqualTo(_dateOnly));
        }

        [Test]
        public void DateOnlyConverter_To()
        {
            // Arrange
            var converter = new DateOnlyConverter();

            // Act
            var output = converter.ConvertToProviderExpression.Compile().Invoke(_dateOnly);

            // Assert
            Assert.That(output, Is.EqualTo(_dateTime));
        }

        [Test]
        public void NullableDateOnlyConverter_From()
        {
            // Arrange
            var converter = new NullableDateOnlyConverter();

            // Act
            var output = converter.ConvertFromProviderExpression.Compile().Invoke(_dateTime);

            // Assert
            Assert.That(output, Is.EqualTo(_dateOnly));
        }

        [Test]
        public void NullableDateOnlyConverter_To()
        {
            // Arrange
            var converter = new NullableDateOnlyConverter();

            // Act
            var output = converter.ConvertToProviderExpression.Compile().Invoke(_dateOnly);

            // Assert
            Assert.That(output, Is.EqualTo(_dateTime));
        }

        [Test]
        public void NullableDateOnlyConverter_From_Null()
        {
            // Arrange
            var converter = new NullableDateOnlyConverter();

            // Act
            var output = converter.ConvertFromProviderExpression.Compile().Invoke(null);

            // Assert
            Assert.That(output, Is.Null);
        }

        [Test]
        public void NullableDateOnlyConverter_To_Null()
        {
            // Arrange
            var converter = new NullableDateOnlyConverter();

            // Act
            var output = converter.ConvertToProviderExpression.Compile().Invoke(null);

            // Assert
            Assert.That(output, Is.Null);
        }
    }
}