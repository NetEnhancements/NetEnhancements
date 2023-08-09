using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using ClosedXML.Excel;

using NetEnhancements.Util;

namespace NetEnhancements.ClosedXML;

internal static class PropertyParser
{
    /// <summary>
    /// Parse a class's properties' <see cref="ColumnAttribute"/> to determine in which Excel column its data resides.
    /// </summary>
    public static Dictionary<int, PropertyTypeInfo> ParseProperties<TRow>()
        where TRow : class, new()
    {
        var cache = new Dictionary<int, PropertyTypeInfo>();

        var rowObjectTypeName = typeof(TRow).FullName;
        var columnAttributeName = nameof(ColumnAttribute).RemoveEnd(nameof(Attribute));

        var properties = typeof(TRow).GetProperties();

        foreach (var property in properties)
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

            if (string.IsNullOrWhiteSpace(columnAttribute?.Name))
            {
                continue;
            }

            var columnIndex = ColumnExtensions.LetterToIndex(columnAttribute.Name);

            if (cache.TryGetValue(columnIndex, out var otherProperty))
            {
                var thisPropertyName = rowObjectTypeName + "." + property.Name;
                var otherPropertyName = rowObjectTypeName + "." + otherProperty.PropertyInfo.Name;
                var attributeDescription = $"[{columnAttributeName}(\"{columnAttribute.Name}\")]";

                var errorString = $"Property '{thisPropertyName}' has {attributeDescription}, but '{otherPropertyName}' already claimed that column.";

                throw new InvalidOperationException(errorString);
            }

            cache[columnIndex] = GetPropertyTypeInfo(property);
        }

        if (!cache.Any())
        {
            throw new InvalidOperationException($"No [{columnAttributeName}] found on {rowObjectTypeName}");
        }

        return cache;
    }

    /// <summary>
    /// Parse a class's properties' <see cref="ExcelColumnNameAttribute"/> to determine into which Excel column name to write its data.
    /// </summary>
    public static Dictionary<string, WritePropertyTypeInfo> ParseWriteProperties<TRow>()
        where TRow : class, new()
    {
        var cache = new Dictionary<string, WritePropertyTypeInfo>();

        var rowObjectTypeName = typeof(TRow).FullName;
        var columnAttributeName = nameof(ColumnAttribute).RemoveEnd(nameof(Attribute));

        var properties = typeof(TRow).GetProperties();

        foreach (var property in properties)
        {
            var attrsDisabled = property.GetCustomAttributes(typeof(ExcelColumnDisabled), true).FirstOrDefault();

            if (attrsDisabled is ExcelColumnDisabled)
            {
                continue;
            }

            var attrs = property.GetCustomAttributes(typeof(ExcelColumnName), true).FirstOrDefault();

            cache.Add(columnAttribute?.ColumnName ?? property.Name, GetWritePropertyTypeInfo(property));
        }

        if (!cache.Any())
        {
            throw new InvalidOperationException($"No [{columnAttributeName}] found on {rowObjectTypeName}");
        }

        return cache;
    }

    private static PropertyTypeInfo GetPropertyTypeInfo(PropertyInfo property)
    {
        var (type, nullable) = GetPropertyType(property);

        return new PropertyTypeInfo(property, type, nullable);
    }
    private static WritePropertyTypeInfo GetWritePropertyTypeInfo(PropertyInfo property)
    {
        var (type, nullable) = GetPropertyType(property);
        
        var columnFormat = property.GetCustomAttribute<ExcelColumnStyleAttribute>(inherit: true);

        if (columnFormat != null)
        {
            return new WritePropertyTypeInfo(property, type, nullable, columnFormat.HorizontalAlignment, columnFormat.VerticalAlignment, columnFormat.TopBorder, columnFormat.BottomBorder, columnFormat.LeftBorder, columnFormat.RightBorder, columnFormat.TopBorderColor, columnFormat.BottomBorderColor, columnFormat.LeftBorderColor, columnFormat.RightBorderColor, columnFormat.FillColor, columnFormat.FontBold, columnFormat.DateFormat, columnFormat.IncludeQuotePrefix, columnFormat.NumberFormat, columnFormat.Protection, columnFormat.SetIncludeQuotePrefix);

        }


        return new WritePropertyTypeInfo(property, type, nullable);
    }

    /// <summary>
    /// Parse the <see cref="PropertyInfo"/> into a usable type and a flag whether it's a nullable value type.
    /// </summary>
    private static (CellType CellType, bool IsNullable) GetPropertyType(PropertyInfo property)
    {
        var type = property.PropertyType;

        var isNullable = type.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

        if (isNullable)
        {
            type = type.GenericTypeArguments[0];
        }

        var cellType = GetCellType(type, property);

        return (cellType, isNullable);
    }

    private static CellType GetCellType(Type type, PropertyInfo property)
    {
        if (type == typeof(string))
            return CellType.Text;

        if (type == typeof(DateTime))
            return CellType.DateTime;

        if (type == typeof(int))
            return CellType.Integer;

        if (type == typeof(decimal))
            return CellType.Decimal;

        if (type == typeof(bool))
            return CellType.Boolean;

        throw new ArgumentException($"Invalid property type '{property.PropertyType.FullName}' for '{property.DeclaringType?.FullName}.{property.Name}'");
    }
}
