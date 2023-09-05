using System.Reflection;

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

internal class WritePropertyTypeInfo 
{
    public IReadOnlyCollection<ExcelColumnConditionalStyleAttribute> ConditionalStyleAttributes { get; }

    public ExcelColumnStyleAttribute? ExcelColumnStyle { get; }

    public PropertyInfo PropertyInfo { get; }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
        ConditionalStyleAttributes = Array.Empty<ExcelColumnConditionalStyleAttribute>();
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, ExcelColumnStyleAttribute? excelColumnStyleAttribute)
    {
        PropertyInfo = propertyInfo;
        ExcelColumnStyle = excelColumnStyleAttribute;
        ConditionalStyleAttributes = Array.Empty<ExcelColumnConditionalStyleAttribute>();
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, ExcelColumnStyleAttribute? excelColumnStyleAttribute, IReadOnlyCollection<ExcelColumnConditionalStyleAttribute> columnConditionalStyle)
    {
        PropertyInfo = propertyInfo;
        ExcelColumnStyle = excelColumnStyleAttribute;
        ConditionalStyleAttributes = columnConditionalStyle;
    }
}
