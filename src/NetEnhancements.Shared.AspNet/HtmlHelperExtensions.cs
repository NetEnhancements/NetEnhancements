using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetEnhancements.Shared.AspNet
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Safely print user input (encoding ALL the things), but with newlines converted to newlines with a {br/}.
        /// </summary>
        public static IHtmlContent RawWithLineBreaks(this IHtmlHelper helper, string? text)
        {
            // First escape,
            var encoded = helper.Encode(text);
            // then add <br/>,
            encoded = encoded.Replace(helper.Encode('\n'), "\n<br/>");
            // return as Raw so it won't get encoded again.
            return helper.Raw(encoded);
        }
    }
}
