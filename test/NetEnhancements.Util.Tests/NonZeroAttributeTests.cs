using NetEnhancements.Util.Validation;

namespace NetEnhancements.Util.Tests
{
    public class NonZeroAttributeTests
    {
        private class Foo
        {
            [NonZero]
            public decimal Bar { get; set; }
        }

        private class FooCustomError
        {
            [NonZero("Custom error")]
            public decimal Baz { get; set; }
        }

        [Test]
        public void NonZeroAttribute_Reports_MemberErrorWithZeroMessage()
        {
            // Arrange
            var foo = new Foo();

            // Act
            var (isValid, errors) = AttributeValidator.Validate(foo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isValid, Is.False);
                Assert.That(errors, Has.Count.EqualTo(1));
                Assert.That(errors[0].MemberNames, Contains.Item("Bar"));
                Assert.That(errors[0].ErrorMessage, Contains.Substring("zero"));
            });
        }

        [Test]
        public void NonZeroAttribute_Reports_CustomErrorMessage()
        {
            // Arrange
            var foo = new FooCustomError();

            // Act
            var (isValid, errors) = AttributeValidator.Validate(foo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isValid, Is.False);
                Assert.That(errors, Has.Count.EqualTo(1));
                Assert.That(errors[0].MemberNames, Contains.Item("Baz"));
                Assert.That(errors[0].ErrorMessage, Is.EqualTo("Custom error"));
            });
        }

        [Test]
        public void NonZeroAttribute_Reports_Valid()
        {
            // Arrange
            var foo = new Foo
            {
                Bar = 42,
            };

            // Act
            var (isValid, errors) = AttributeValidator.Validate(foo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isValid, Is.True);
                Assert.That(errors, Is.Empty);
            });
        }
    }
}
