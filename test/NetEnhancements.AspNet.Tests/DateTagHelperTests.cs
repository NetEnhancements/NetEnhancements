using Microsoft.AspNetCore.Razor.TagHelpers;
using NetEnhancements.AspNet.TagHelpers;
using NetEnhancements.Util;

namespace NetEnhancements.AspNet.Tests
{
    public class DateTagHelperTests
    {
        [Test]
        public void Process_Prints_Value()
        {
            // Arrange
            var dateTime = (DateTimeOffset)new DateTime(2023, 03, 31, 09, 21, 42);
            var dateTimeString = dateTime.ToReadableString().Replace(" ", "&nbsp;");

            var classUnderTest = new DateTagHelper
            {
                Value = dateTime
            };

            var otherAttributes = new TagHelperAttributeList();
            var context = new TagHelperContext("date", otherAttributes, new Dictionary<object, object>(), "1");
            var output = new TagHelperOutput("date", otherAttributes, (_, _) => Task.FromResult<TagHelperContent?>(null));

            // Act
            classUnderTest.Process(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.That(content, Is.EqualTo(dateTimeString));
        }

        [Test]
        public void Process_Prints_ReplacementValue()
        {
            // Arrange
            const string replacement = "never";
            var classUnderTest = new DateTagHelper
            {
                Value = null,
                ValueIfNull = replacement
            };

            var otherAttributes = new TagHelperAttributeList();
            var context = new TagHelperContext("date", otherAttributes, new Dictionary<object, object>(), "1");
            var output = new TagHelperOutput("date", otherAttributes, (_, _) => Task.FromResult<TagHelperContent?>(null));

            // Act
            classUnderTest.Process(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.That(content, Is.EqualTo(replacement));
        }
    }
}
