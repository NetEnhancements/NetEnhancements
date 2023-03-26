using System;
using NetEnhancements.Util;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetEnhancements.Shared.AspNet.TagHelpers
{
    [HtmlTargetElement("date")]
    public class DateTagHelper : TagHelper
    {
        public DateTimeOffset? Value { get; set; }

        public string? ValueIfNull { get; set; }

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

            var readableDateString = Value.Value.ToReadableString().Replace(" ", "&nbsp;");

            output.Content.SetHtmlContent(readableDateString);
        }
    }
}
