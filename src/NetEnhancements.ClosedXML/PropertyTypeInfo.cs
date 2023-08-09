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

    public PropertyInfo PropertyInfo { get; init; }

    public CellType CellType { get; init; }

    public bool IsNullable { get; init; }
}

internal class WritePropertyTypeInfo : PropertyTypeInfo
{
    public XLAlignmentHorizontalValues? HorizontalAlignment { get; set; }
    public XLAlignmentVerticalValues? VerticalAlignment { get; set; }

    public XLBorderStyleValues? TopBorder { get; set; } 
    public XLBorderStyleValues? BottomBorder { get; set; }
    public XLBorderStyleValues? LeftBorder { get; set; } 
    public XLBorderStyleValues? RightBorder { get; set; }

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
        DateFormat = dateFormat;
        FillColor = fillColor;
        FontColor = fontColor;
        FontBold = fontBold;
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
