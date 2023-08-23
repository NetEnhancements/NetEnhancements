using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Extension method container.
    /// </summary>
    public static class CellExtensions
    {
        /// <summary>
        /// Read a cell's value, returning a default value when unable to.
        /// </summary>
        public static TValue? GetNullableValue<TValue>(this IXLCell cell, TValue? defaultValue = default)
            where TValue : IComparable<TValue>
        {
            return cell.TryGetValue(out TValue value) ? value ?? defaultValue : defaultValue;
        }

        internal static void ApplyStyleAttribute(this IXLStyle style, ExcelColumnStyleAttribute column)
        {
            if (!string.IsNullOrEmpty(column.NumberFormat))
            {
                style.NumberFormat.Format = column.NumberFormat;
            }

            if (!string.IsNullOrEmpty(column.DateFormat))
            {
                style.DateFormat.Format = column.DateFormat;
            }

            if (column.IsHorizontalAlignmentSet)
            {
                style.Alignment.Horizontal = column.HorizontalAlignment;
            }

            if (column.IsVerticalAlignmentSet)
            {
                style.Alignment.Vertical = column.VerticalAlignment;
            }

            if (column.IsTopBorderSet)
            {
                style.Border.TopBorder = column.TopBorder;
            }
            if (column.IsBottomBorderSet)
            {
                style.Border.BottomBorder = column.BottomBorder;
            }
            if (column.IsLeftBorderSet)
            {
                style.Border.LeftBorder = column.LeftBorder;
            }
            if (column.IsRightBorderSet)
            {
                style.Border.RightBorder = column.RightBorder;
            }

            if (!string.IsNullOrEmpty(column.TopBorderColor))
            {
                style.Border.TopBorderColor = XLColor.FromHtml(column.TopBorderColor);
            }
            if (!string.IsNullOrEmpty(column.BottomBorderColor))
            {
                style.Border.BottomBorderColor = XLColor.FromHtml(column.BottomBorderColor);
            }
            if (!string.IsNullOrEmpty(column.LeftBorderColor))
            {
                style.Border.LeftBorderColor = XLColor.FromHtml(column.LeftBorderColor);
            }
            if (!string.IsNullOrEmpty(column.RightBorderColor))
            {
                style.Border.RightBorderColor = XLColor.FromHtml(column.RightBorderColor);
            }

            if (!string.IsNullOrEmpty(column.FillColor))
            {
                style.Fill.BackgroundColor = XLColor.FromHtml(column.FillColor);
            }
            if (!string.IsNullOrEmpty(column.FontColor))
            {
                style.Font.FontColor = XLColor.FromHtml(column.FontColor);
            }

            style.Font.Bold = column.FontBold;

            style.IncludeQuotePrefix = column.IncludeQuotePrefix;
            style.SetIncludeQuotePrefix(column.SetIncludeQuotePrefix);
            style.Protection.Locked = column.IsProtected;
        }
    }
}