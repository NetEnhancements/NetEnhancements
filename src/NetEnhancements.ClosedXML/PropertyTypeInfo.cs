using System.Reflection;

using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML;

internal class PropertyTypeInfo
{
    public PropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
    {
        this.PropertyInfo = propertyInfo;
        this.CellType = cellType;
        this.IsNullable = isNullable;
    }

    public PropertyInfo PropertyInfo { get; init; }

    public CellType CellType { get; init; }

    public bool IsNullable { get; init; }

    public IXLStyle Style { get; init; }

    public void Deconstruct(out PropertyInfo propertyInfo, out CellType cellType, out bool isNullable, out IXLStyle style)
    {
        propertyInfo = this.PropertyInfo;
        cellType = this.CellType;
        isNullable = this.IsNullable;
        style = this.Style;
    }
}

internal class WritePropertyTypeInfo : PropertyTypeInfo
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

    public string DateFormat { get; }

    public XLColor FillColor { get; set; } = XLColor.FromArgb(255, 0, 0, 0);

    public bool FontBold { get; set; }

    public bool IncludeQuotePrefix { get; set; }

    public string NumberFormat { get; set; }

    public IXLProtection Protection { get; set; }

    public bool SetIncludeQuotePrefix { get; set; } = true;

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable,
                                 XLAlignmentHorizontalValues horizontalAlignment,
                                 XLAlignmentVerticalValues verticalAlignment,
                                 XLBorderStyleValues topBorder,
                                 XLBorderStyleValues bottomBorder,
                                 XLBorderStyleValues leftBorder,
                                 XLBorderStyleValues rightBorder,
                                 XLColor topBorderColor,
                                 XLColor bottomBorderColor,
                                 XLColor leftBorderColor,
                                 XLColor rightBorderColor,
                                 XLColor fillColor,
                                 bool fontBold,
                                 string dateFormat,
                                 bool includeQuotePrefix,
                                 string numberFormat,
                                 IXLProtection protection,
                                 bool setIncludeQuotePrefix)
        : base(propertyInfo, cellType, isNullable)
    {
        this.HorizontalAlignment = horizontalAlignment;
        this.VerticalAlignment = verticalAlignment;
        this.TopBorder = topBorder;
        this.BottomBorder = bottomBorder;
        this.LeftBorder = leftBorder;
        this.RightBorder = rightBorder;
        this.TopBorderColor = topBorderColor;
        this.BottomBorderColor = bottomBorderColor;
        this.LeftBorderColor = leftBorderColor;
        this.RightBorderColor = rightBorderColor;
        this.DateFormat = dateFormat;
        this.FillColor = fillColor;
        this.FontBold = fontBold;
        this.IncludeQuotePrefix = includeQuotePrefix;
        this.NumberFormat = numberFormat;
        this.Protection = protection;
        this.SetIncludeQuotePrefix = setIncludeQuotePrefix;
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
        : base(propertyInfo, cellType, isNullable)
    {
    }

    public void Deconstruct(out PropertyInfo propertyInfo, out CellType cellType, out bool isNullable, out IXLStyle style)
    {
        propertyInfo = this.PropertyInfo;
        cellType = this.CellType;
        isNullable = this.IsNullable;
        style = this.Style;
    }
}
