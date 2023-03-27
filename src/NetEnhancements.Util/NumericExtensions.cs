namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for numeric types.
    /// </summary>
    public static class NumericExtensions
    {
        public static string ToValueString(this decimal value) =>
            value.HasDecimals()
                ? value.ToString(".00################")
                : value.ToString("0");

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
        /// Returns <c>true</c> when the decimal does not end in .0.
        /// </summary>
        public static bool HasDecimals(this decimal d) => d % 1 != 0;
    }
}
