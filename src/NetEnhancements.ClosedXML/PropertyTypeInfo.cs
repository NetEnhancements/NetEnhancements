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
}

internal class WritePropertyTypeInfo : PropertyTypeInfo
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

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable,
                                 XLAlignmentHorizontalValues horizontalAlignment,
                                 XLAlignmentVerticalValues verticalAlignment,
                                 XLBorderStyleValues topBorder,
                                 XLBorderStyleValues bottomBorder,
                                 XLBorderStyleValues leftBorder,
                                 XLBorderStyleValues rightBorder,
                                 string? topBorderColor,
                                 string? bottomBorderColor,
                                 string? leftBorderColor,
                                 string? rightBorderColor,
                                 string? fillColor,
                                 string? fontColor,
                                 bool fontBold,
                                 string? dateFormat,
                                 bool includeQuotePrefix,
                                 string? numberFormat,
                                 bool isProtected,
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
        this.FontColor = fontColor;
        this.FontBold = fontBold;
        this.IncludeQuotePrefix = includeQuotePrefix;
        this.NumberFormat = numberFormat;
        this.IsProtected = isProtected;
        this.SetIncludeQuotePrefix = setIncludeQuotePrefix;
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
        : base(propertyInfo, cellType, isNullable)
    {
    }
}
