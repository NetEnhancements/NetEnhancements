using System.Reflection;

using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML;

internal class PropertyTypeInfo
{
    public PropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
    {
        PropertyInfo = propertyInfo;
        CellType = cellType;
        IsNullable = isNullable;
    }

    public PropertyInfo PropertyInfo { get; }

    public CellType CellType { get; }

    public bool IsNullable { get; }
}

internal class WritePropertyTypeInfo : PropertyTypeInfo
{
    public XLAlignmentHorizontalValues? HorizontalAlignment { get; }
    public XLAlignmentVerticalValues? VerticalAlignment { get; }

    public XLBorderStyleValues? TopBorder { get; } 
    public XLBorderStyleValues? BottomBorder { get; }
    public XLBorderStyleValues? LeftBorder { get; } 
    public XLBorderStyleValues? RightBorder { get; }

    public string? TopBorderColor { get; } 
    public string? BottomBorderColor { get; } 
    public string? LeftBorderColor { get; }
    public string? RightBorderColor { get; } 

    public string? DateFormat { get; }

    public string? FillColor { get; } 
    public string? FontColor { get; } 

    public bool FontBold { get; }

    public bool IncludeQuotePrefix { get; }

    public string? NumberFormat { get; }

    public bool IsProtected { get; }

    public bool SetIncludeQuotePrefix { get; } = true;

    public WritePropertyTypeInfo(
        PropertyInfo propertyInfo,
        CellType cellType, 
        bool isNullable,
        XLAlignmentHorizontalValues? horizontalAlignment,
        XLAlignmentVerticalValues? verticalAlignment,
        XLBorderStyleValues? topBorder,
        XLBorderStyleValues? bottomBorder,
        XLBorderStyleValues? leftBorder,
        XLBorderStyleValues? rightBorder,
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
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = verticalAlignment;
        TopBorder = topBorder;
        BottomBorder = bottomBorder;
        LeftBorder = leftBorder;
        RightBorder = rightBorder;
        TopBorderColor = topBorderColor;
        BottomBorderColor = bottomBorderColor;
        LeftBorderColor = leftBorderColor;
        RightBorderColor = rightBorderColor;
        FillColor = fillColor;
        FontColor = fontColor;
        FontBold = fontBold;
        DateFormat = dateFormat;
        IncludeQuotePrefix = includeQuotePrefix;
        NumberFormat = numberFormat;
        IsProtected = isProtected;
        SetIncludeQuotePrefix = setIncludeQuotePrefix;
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
        : base(propertyInfo, cellType, isNullable)
    {
    }
}
