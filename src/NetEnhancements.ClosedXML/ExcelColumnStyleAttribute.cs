using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom column name when writing to Excel.
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

        public XLColor TopBorderColor { get; set; } = XLColor.FromArgb(255, 0, 0, 0);
        public XLColor BottomBorderColor { get; set; } = XLColor.FromArgb(255, 0, 0, 0);
        public XLColor LeftBorderColor { get; set; } = XLColor.FromArgb(255, 0, 0, 0);
        public XLColor RightBorderColor { get; set; } = XLColor.FromArgb(255, 0, 0, 0);

        public string? DateFormat { get; }

        public XLColor FillColor { get; set; } = XLColor.FromArgb(255, 0, 0, 0);
        public XLColor FontColor { get; set; } = XLColor.Black;

        public bool FontBold { get; set; }

        public bool IncludeQuotePrefix { get; set; }

        public string? NumberFormat { get; set; }

        public bool IsProtected { get; set; }

        public bool SetIncludeQuotePrefix { get; set; } = true;
    }
}
