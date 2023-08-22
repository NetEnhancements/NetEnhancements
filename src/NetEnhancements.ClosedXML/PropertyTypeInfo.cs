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
    public ExcelColumnConditionalStyleAttribute? ExcelColumnConditionalStyle { get; set; }
    public ExcelColumnStyleAttribute? ExcelColumnStyle { get; set; }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable, ExcelColumnStyleAttribute excelColumnStyleAttribute)
        : base(propertyInfo, cellType, isNullable)
    {
        ExcelColumnStyle = excelColumnStyleAttribute;
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable, ExcelColumnStyleAttribute excelColumnStyleAttribute, ExcelColumnConditionalStyleAttribute columnConditionalStyle)
           : base(propertyInfo, cellType, isNullable)
    {
        ExcelColumnStyle = excelColumnStyleAttribute;
        ExcelColumnConditionalStyle = columnConditionalStyle;
    }

    public WritePropertyTypeInfo(PropertyInfo propertyInfo, CellType cellType, bool isNullable)
        : base(propertyInfo, cellType, isNullable)
    {
    }
}
