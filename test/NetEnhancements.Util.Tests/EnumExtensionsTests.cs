using System.ComponentModel.DataAnnotations;

namespace NetEnhancements.Util.Tests
{
    public class EnumExtensionsTests
    {
        private enum Foo
        {
            [Display(Name = "Foo-1")]
            Foo1,

            Foo2,
        }

        [Test]
        public void GetDisplayName_Reads_Attribute()
        {
            // Arrange & Act
            var name = Foo.Foo1.GetDisplayName();

            // Assert
            Assert.That(name, Is.EqualTo("Foo-1"));
        }

        [Test]
        public void GetDisplayName_Returns_MemberName()
        {
            // Arrange & Act
            var name = Foo.Foo2.GetDisplayName();

            // Assert
            Assert.That(name, Is.EqualTo("Foo2"));
        }

        [Test]
        public void GetDisplayName_Returns_Value()
        {
            // Arrange & Act
            var name = ((Foo)42).GetDisplayName();

            // Assert
            Assert.That(name, Is.EqualTo("42"));
        }
    }
}
