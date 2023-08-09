using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom style name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnStyleAttribute : Attribute
    {
        public XLAlignmentHorizontalValues HorizontalAlignment { get; set; } = XLAlignmentHorizontalValues.Left;
        public XLAlignmentVerticalValues VerticalAlignment { get; set; } = XLAlignmentVerticalValues.Top;

        public XLBorderStyleValues TopBorder { get; set; } = XLBorderStyleValues.None;
        public XLBorderStyleValues BottomBorder { get; set; } = XLBorderStyleValues.None;
        public XLBorderStyleValues LeftBorder { get; set; } = XLBorderStyleValues.None;
        public XLBorderStyleValues RightBorder { get; set; } = XLBorderStyleValues.None;

        public string? TopBorderColor { get; set; }
        public string? BottomBorderColor { get; set; }
        public string? LeftBorderColor { get; set; }
        public string? RightBorderColor { get; set; }

        public string? DateFormat { get; }

        public string? FillColor { get; set; }
        public string? FontColor { get; set; }

        public bool FontBold { get; set; }

        public bool IncludeQuotePrefix { get; set; }

        public string? NumberFormat { get; set; }

        public bool IsProtected { get; set; }

        public bool SetIncludeQuotePrefix { get; set; } = true;
    }
}
