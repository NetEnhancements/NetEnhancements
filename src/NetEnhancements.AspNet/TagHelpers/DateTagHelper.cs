using System.Web;
using NetEnhancements.Util;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetEnhancements.AspNet.TagHelpers
{
    /// <summary>
    /// Prints a date and time value in a human-readable format, with the original date and time as an abbr tag's title.
    /// </summary>
    [HtmlTargetElement("date")]
    public class DateTagHelper : TagHelper
    {
        /// <summary>
        /// The date and time to print.
        /// </summary>
        public DateTimeOffset? Value { get; set; }

        /// <summary>
        /// The value to output if the <see cref="Value"/> is <c>null</c>.
        /// </summary>
        public string? ValueIfNull { get; set; }

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // <abbr> with title attribute for mouseover.
            output.TagName = "abbr";

            if (!Value.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(ValueIfNull))
                {
                    output.Content.SetHtmlContent(ValueIfNull);
                }

                return;
            }

            var iso8601DateTimeString = Value.Value.ToString("O");

            output.Attributes.Add("data-date", iso8601DateTimeString);

            output.Attributes.Add("title", iso8601DateTimeString);

            output.Attributes.Add("class", "nodots");

            var readableDateString = Value.Value.ToReadableString();

            // First encode anything that might mean something...
            readableDateString = HttpUtility.HtmlEncode(readableDateString);

            // And _then_ replace spaces with non-breaking ones.
            readableDateString = readableDateString.Replace(" ", "&nbsp;");

            output.Content.SetHtmlContent(readableDateString);
        }
    }
}
