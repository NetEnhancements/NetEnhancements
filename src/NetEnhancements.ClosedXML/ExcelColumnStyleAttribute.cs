using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom style name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnStyleAttribute : Attribute
    {
        /// <summary>Sets the cell's horizontal alignment.</summary>
        public XLAlignmentHorizontalValues HorizontalAlignment
        {
            get =>
                _internalHorizontalAlignment.HasValue
                    ? (XLAlignmentHorizontalValues)_internalHorizontalAlignment.Value
                    : default;
            init => _internalHorizontalAlignment = (int)value;
        }

        private readonly int? _internalHorizontalAlignment;

        internal bool IsHorizontalAlignmentSet => _internalHorizontalAlignment.HasValue;

        /// <summary>Sets the cell's vertical alignment.</summary>
        public XLAlignmentVerticalValues VerticalAlignment
        {
            get =>
                _internalVerticalAlignment.HasValue
                    ? (XLAlignmentVerticalValues)_internalVerticalAlignment.Value
                    : default;
            init => _internalVerticalAlignment = (int)value;
        }

        private readonly int? _internalVerticalAlignment;

        internal bool IsVerticalAlignmentSet => _internalVerticalAlignment.HasValue;

        /// <summary>Sets the cell's top border.</summary>
        public XLBorderStyleValues TopBorder
        {
            get =>
                _internalTopBorder.HasValue
                    ? (XLBorderStyleValues)_internalTopBorder.Value
                    : default;
            init => _internalTopBorder = (int)value;
        }

        private readonly int? _internalTopBorder;

        internal bool IsTopBorderSet => _internalTopBorder.HasValue;
        
        /// <summary>Sets the cell's bottom border.</summary>
        public XLBorderStyleValues BottomBorder
        {
            get =>
                _internalBottomBorder.HasValue
                    ? (XLBorderStyleValues)_internalBottomBorder.Value
                    : default;
            init => _internalBottomBorder = (int)value;
        }

        private readonly int? _internalBottomBorder;

        internal bool IsBottomBorderSet => _internalBottomBorder.HasValue;
        
        /// <summary>Sets the cell's left border.</summary>
        public XLBorderStyleValues LeftBorder
        {
            get =>
                _internalLeftBorder.HasValue
                    ? (XLBorderStyleValues)_internalLeftBorder.Value
                    : default;
            init => _internalLeftBorder = (int)value;
        }

        private readonly int? _internalLeftBorder;

        internal bool IsLeftBorderSet => _internalLeftBorder.HasValue;
        
        /// <summary>Sets the cell's right border.</summary>
        public XLBorderStyleValues RightBorder
        {
            get =>
                _internalRightBorder.HasValue
                    ? (XLBorderStyleValues)_internalRightBorder.Value
                    : default;
            init => _internalRightBorder = (int)value;
        }

        private readonly int? _internalRightBorder;

        internal bool IsRightBorderSet => _internalRightBorder.HasValue;
        
        /// <summary>Sets the cell's top border color using a hex value.</summary>
        public string? TopBorderColor { get; set; }
        /// <summary>Sets the cell's bottom border color using a hex value.</summary>
        public string? BottomBorderColor { get; set; }
        /// <summary>Sets the cell's left border color using a hex value.</summary>
        public string? LeftBorderColor { get; set; }
        /// <summary>Sets the cell's right border color using a hex value.</summary>
        public string? RightBorderColor { get; set; }

        /// <summary>Sets the cell's date format.</summary>
        public string? DateFormat { get; set; }

        /// <summary>Sets the cell's background color using a hex value.</summary>
        public string? FillColor { get; set; }
        /// <summary>Sets the cell's font color using a hex value.</summary>
        public string? FontColor { get; set; }

        /// <summary>Sets the cell's font weight to bold.</summary>
        public bool FontBold { get; set; }

        /// <summary> Should the text values of a cell saved to the file be prefixed by a quote (<c>'</c>) character? </summary>
        public bool IncludeQuotePrefix { get; set; }

        /// <summary>Sets the cell's number format.</summary>
        public string? NumberFormat { get; set; }

        /// <summary>Sets the cell's locked state.</summary>
        public bool IsProtected { get; set; }

        /// <summary> Should the text values of a cell saved to the file be prefixed by a quote (<c>'</c>) character? </summary>
        public bool SetIncludeQuotePrefix { get; set; } = true;
    }
}
