using System.Text.Json;
using System.Text.Json.Serialization;
using NetEnhancements.Util.Json;

namespace NetEnhancements.Util.Tests.Json
{
    public class ShouldSerializeModifierTests
    {
        private class PropertyTest
        {
            public string? Foo { get; set; }

            public bool ShouldSerializeFoo => !string.IsNullOrEmpty(Foo);
        }

        private class MethodTest
        {
            public string? Foo { get; set; }
            public bool ShouldSerializeFoo() => !string.IsNullOrEmpty(Foo);
        }

        private class IgnoreTest
        {
            [JsonIgnore]
            public string? IgnoreMe { get; set; }
            public bool ShouldSerializeIgnoreMe => true;
        }

        [Test]
        public void Serialize_Calls_ShouldSerializeProperty()
        {
            // Arrange
            var foo = new PropertyTest { Foo = "42" };

            var options = new JsonSerializerOptions().WithShouldSerializeModifier();

            // Act
            string json = JsonSerializer.Serialize(foo, options);

            // Assert
            Assert.That(json, Is.EqualTo(/*lang=json,strict*/ """{"Foo":"42"}"""));
        }

        [Test]
        public void Serialize_Calls_ShouldSerializeMethod()
        {
            // Arrange
            var foo = new MethodTest { Foo = "42" };

            var options = new JsonSerializerOptions().WithShouldSerializeModifier();

            // Act
            string json = JsonSerializer.Serialize(foo, options);

            // Assert
            Assert.That(json, Is.EqualTo(/*lang=json,strict*/ """{"Foo":"42"}"""));
        }
    }
}
