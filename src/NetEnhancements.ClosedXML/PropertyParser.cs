using System.Reflection;
using NetEnhancements.Util;

namespace NetEnhancements.ClosedXML;

internal static class PropertyParser
{
    /// <summary>
    /// Parse a class's properties' <see cref="ExcelColumnNameAttribute"/> to determine in which Excel column its data resides.
    /// </summary>
    public static Dictionary<int, PropertyTypeInfo> ParseReadProperties<TRow>()
        where TRow : class, new()
    {
        var cache = new Dictionary<int, PropertyTypeInfo>();

        var rowObjectTypeName = typeof(TRow).FullName;
        var columnAttributeName = nameof(ExcelColumnNameAttribute).RemoveEnd(nameof(Attribute));

        var properties = typeof(TRow).GetProperties();

        foreach (var property in properties)
        {
            var columnAttribute = property.GetCustomAttribute<ExcelColumnNameAttribute>();

            if (string.IsNullOrWhiteSpace(columnAttribute?.ColumnName))
            {
                continue;
            }

            var columnIndex = ColumnExtensions.LetterToIndex(columnAttribute.ColumnName);

            if (cache.TryGetValue(columnIndex, out var otherProperty))
            {
                var thisPropertyName = rowObjectTypeName + "." + property.Name;
                var otherPropertyName = rowObjectTypeName + "." + otherProperty.PropertyInfo.Name;
                var attributeDescription = $"[{columnAttributeName}(\"{columnAttribute.ColumnName}\")]";

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

    public static Dictionary<string, PropertyInfo> ParsePropertiesToColumnNames<TRow>()
        where TRow : class, new()
    {
        var cache = new Dictionary<string, PropertyInfo>();

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

            if (attrs is ExcelColumnName excelColumnName)
            {
                cache.Add(excelColumnName.ColumnName, property);
            }
            else
            {
                cache.Add(property.Name, property);
            }
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
