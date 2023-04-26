using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML;

/// <summary>
/// Parses an <see cref="IXLRow"/> into the given <typeparamref name="TRow"/> type.
/// </summary>
public class RowParser<TRow>
    where TRow : class, new()
{
    private readonly Dictionary<int, PropertyTypeInfo> _propertyCache;
    
    /// <summary>
    ///Initialize a new instance.
    /// </summary>
    public RowParser()
    {
        _propertyCache = PropertyParser.ParseReadProperties<TRow>();
    }

    /// <summary>
    /// Parse an <see cref="IXLRow"/> into the given <typeparamref name="TRow"/> type.
    /// </summary>
    public TRow ParseRow(IXLRow row)
    {
        var rowData = new TRow();

        foreach (var cell in row.CellsUsed())
        {
            if (!_propertyCache.TryGetValue(cell.Address.ColumnNumber, out var property))
            {
                continue;
            }

            SetPropertyValue(rowData, property, cell);
        }

        return rowData;
    }

    private static void SetPropertyValue(TRow rowData, PropertyTypeInfo property, IXLCell cell)
    {
        object value = property.CellType switch
        {
            CellType.Text => cell.Value.ToString(),
            
            CellType.Decimal => property.IsNullable ? cell.GetNullableValue<decimal>() : cell.GetValue<decimal>(),
            CellType.Integer => property.IsNullable ? cell.GetNullableValue<int>() : cell.GetValue<int>(),

            CellType.Boolean => throw new NotImplementedException("TODO: support boolean"),

            CellType.DateTime => throw new NotImplementedException("TODO: support DateTime"),

            _ => throw new ArgumentOutOfRangeException($"Unknown type '{property.CellType}'")
        };

        property.PropertyInfo.SetValue(rowData, value);
    }
}