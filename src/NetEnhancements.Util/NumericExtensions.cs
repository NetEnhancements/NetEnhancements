using System;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for numeric types.
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Returns a string representing the given <paramref name="value"/>, with at least 2 decimals when not zero (otherwise returns "0"), more if specified in the <paramref name="value"/>.
        /// </summary>
        public static string ToValueString(this decimal value) =>
            value.HasDecimals()
                ? value.ToString(".00################")
                : value.ToString("0");

        /// <summary>
        /// Returns a string representing the given <paramref name="value"/>, with at least 2 decimals when not zero or null (otherwise returns "0"), more if specified in the <paramref name="value"/>.
        /// </summary>
        public static string ToValueString(this decimal? value) =>
            value.HasValue
                ? value.Value.ToValueString()
                : "0";

        /// <summary>
        /// TODO: which currency is this anyway?
        /// </summary>
        public static string ToPriceString(this decimal value, int decimals = 2, IFormatProvider? cultureInfo = null)
            => ((decimal?)value).ToPriceString(decimals, cultureInfo);

        /// <summary>
        /// TODO: which currency is this anyway?
        /// </summary>
        public static string ToPriceString(this decimal? value, int decimals = 2, IFormatProvider? cultureInfo = null) =>
            value.HasValue
                ? value.Value.ToString($"N{decimals}", cultureInfo)
                : "-";

        /// <summary>
        /// Returns <c>true</c> when the decimal has decimals after the dot.
        /// </summary>
        /// <devdoc>
        /// Implementation detail: a decimal is stored as four integers, the fourth of which ([3]) contains the number of decimals.
        /// In those upper 2 bytes of that integer, masked with `01111111` (127, 0x7F) to get the lower 7 bits of that, is the number of decimals.
        /// </devdoc>
        public static bool HasDecimals(this decimal value) => ((decimal.GetBits(value)[3] >> 16) & 0x7F) > 0;
    }
}
