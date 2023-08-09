using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom style name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnStyleAttribute : Attribute
    {
        public XLAlignmentHorizontalValues HorizontalAlignment
        {
            get
            {
                return _internalHorizontalAlignment.HasValue
                           ? (XLAlignmentHorizontalValues)_internalHorizontalAlignment.Value
                           : default; // Default value representing the absence of a value
            }
            init => _internalHorizontalAlignment = (int)value;
        }

        private readonly int? _internalHorizontalAlignment;

        internal bool IsHorizontalAlignmentSet => _internalHorizontalAlignment.HasValue;

        public XLAlignmentVerticalValues VerticalAlignment
        {
            get
            {
                return _internalVerticalAlignment.HasValue
                           ? (XLAlignmentVerticalValues)_internalVerticalAlignment.Value
                           : default; // Default value representing the absence of a value
            }
            init => _internalVerticalAlignment = (int)value;
        }

        private readonly int? _internalVerticalAlignment;

        internal bool IsVerticalAlignmentSet => _internalVerticalAlignment.HasValue;

        public XLBorderStyleValues TopBorder
        {
            get
            {
                return _internalTopBorder.HasValue
                           ? (XLBorderStyleValues)_internalTopBorder.Value
                           : default; // Default value representing the absence of a value
            }
            init => _internalTopBorder = (int)value;
        }

        private readonly int? _internalTopBorder;

        internal bool IsTopBorderSet => _internalTopBorder.HasValue;

        public XLBorderStyleValues BottomBorder
        {
            get
            {
                return _internalBottomBorder.HasValue
                           ? (XLBorderStyleValues)_internalBottomBorder.Value
                           : default; // Default value representing the absence of a value
            }
            init => _internalBottomBorder = (int)value;
        }

        private readonly int? _internalBottomBorder;

        internal bool IsBottomBorderSet => _internalBottomBorder.HasValue;

        public XLBorderStyleValues LeftBorder
        {
            get
            {
                return _internalLeftBorder.HasValue
                           ? (XLBorderStyleValues)_internalLeftBorder.Value
                           : default; // Default value representing the absence of a value
            }
            init => _internalLeftBorder = (int)value;
        }

        private readonly int? _internalLeftBorder;

        internal bool IsLeftBorderSet => _internalLeftBorder.HasValue;

        public XLBorderStyleValues RightBorder
        {
            get
            {
                return _internalRightBorder.HasValue
                           ? (XLBorderStyleValues)_internalRightBorder.Value
                           : default; // Default value representing the absence of a value
            }
            init => _internalRightBorder = (int)value;
        }

        private readonly int? _internalRightBorder;

        internal bool IsRightBorderSet => _internalRightBorder.HasValue;

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
