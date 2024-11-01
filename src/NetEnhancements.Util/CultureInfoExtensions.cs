﻿using System.Globalization;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for <see cref="CultureInfo"/>.
    /// </summary>
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// The Dutch culture (tulips 'n such).
        /// </summary>
        public static readonly CultureInfo Dutch = new("nl-NL");

        /// <summary>
        /// Set culture for logging.
        /// </summary>
        public static void SetSensibleCultureInfo()
        {
            var cultureInfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();

            cultureInfo.NumberFormat.CurrencySymbol = "€";

            // DateTime.ToString() "combines the custom format strings returned by the ShortDatePattern and LongTimePattern"
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";

            cultureInfo.DateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
