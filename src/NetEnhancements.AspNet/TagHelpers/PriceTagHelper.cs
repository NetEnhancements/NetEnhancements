using Microsoft.AspNetCore.Razor.TagHelpers;
using NetEnhancements.Util;

namespace NetEnhancements.AspNet.TagHelpers
{
    /// <summary>
    /// qMxX-QOV9tI
    /// </summary>
    public class PriceTagHelper : TagHelper
    {
        public string? Currency { get; set; }

        public decimal? Amount { get; set; }

        public int? Decimals { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            if (string.IsNullOrWhiteSpace(Currency))
            {
                Currency = "€";
            }

            var priceString = Amount.ToPriceString(Decimals ?? 2);

            output.Content.SetHtmlContent(Currency + "&nbsp;" + priceString);
        }
    }
}
