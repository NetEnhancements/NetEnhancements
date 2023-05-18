using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using NetEnhancements.Util.Json;

namespace NetEnhancements.Util.Tests.Json
{
    public class ShouldSerializeExtensionTests
    {
        [Test]
        public void WithShouldSerializeModifier_NullResolver_InstantitesResolver()
        {
            // Arrange
            var options = new JsonSerializerOptions();

            // Act
            options.WithShouldSerializeModifier();

            // Assert
            Assert.That(options.TypeInfoResolver is DefaultJsonTypeInfoResolver, Is.True);
            var resolver = (DefaultJsonTypeInfoResolver)options.TypeInfoResolver!;

            Assert.That(resolver.Modifiers, Has.Count.EqualTo(1));
        }

        [Test]
        public void WithShouldSerializeModifier_DefaultResolver_UsesResolver()
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            // Act
            options.WithShouldSerializeModifier();

            // Assert
            Assert.That(options.TypeInfoResolver is DefaultJsonTypeInfoResolver, Is.True);
            var resolver = (DefaultJsonTypeInfoResolver)options.TypeInfoResolver!;

            Assert.That(resolver.Modifiers, Has.Count.EqualTo(1));
        }

        /// <summary>
        /// Represents a non-null but also non-default TypeInfoResolver, to throw.
        /// </summary>
        private class InvalidResolver : IJsonTypeInfoResolver
        {
            public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options) => throw new NotImplementedException();
        }

        [Test]
        public void WithShouldSerializeModifier_UnsupportedTypeInfoResolver_JsonSerializerOptions_Throws()
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new InvalidResolver()
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => options.WithShouldSerializeModifier());
        }
    }
}
