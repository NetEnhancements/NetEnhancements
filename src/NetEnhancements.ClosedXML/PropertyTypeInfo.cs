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

internal class WritePropertyTypeInfo : PropertyTypeInfo
{
    public IReadOnlyCollection<ExcelColumnConditionalStyleAttribute> ConditionalStyleAttributes { get; }

    public ExcelColumnStyleAttribute? ExcelColumnStyle { get; }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
        : base(propertyInfo, cellType, isNullable)
    {
        ConditionalStyleAttributes = Array.Empty<ExcelColumnConditionalStyleAttribute>();
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable, ExcelColumnStyleAttribute? excelColumnStyleAttribute)
        : base(propertyInfo, cellType, isNullable)
    {
        ExcelColumnStyle = excelColumnStyleAttribute;
        ConditionalStyleAttributes = Array.Empty<ExcelColumnConditionalStyleAttribute>();
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable, ExcelColumnStyleAttribute? excelColumnStyleAttribute, IReadOnlyCollection<ExcelColumnConditionalStyleAttribute> columnConditionalStyle)
           : base(propertyInfo, cellType, isNullable)
    {
        ExcelColumnStyle = excelColumnStyleAttribute;
        ConditionalStyleAttributes = columnConditionalStyle;
    }
}
