using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using ClosedXML.Excel;
using NetEnhancements.Util;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Parses rows into objects.
    /// </summary>
    public static class WorksheetExtensions
    {
        /// <summary>
        /// Read a sheet's used rows into a collection of objects.
        /// </summary>
        public static async IAsyncEnumerable<TRow> ParseRowsAsync<TRow>(this IXLWorksheet worksheet, int rowsToSkip = 1)
            where TRow : class, new()
        {
            var objectParser = new RowParser<TRow>();

            foreach (var row in worksheet.RowsUsed().Skip(rowsToSkip))
            {
                yield return objectParser.ParseRow(row);
            }

            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Parses an <see cref="IXLRow"/> into the given <typeparamref name="TRow"/> type.
    /// </summary>
    public class RowParser<TRow>
        where TRow : class, new()
    {
        private readonly Dictionary<int, PropertyInfo> _propertyCache;

        /// <summary>
        ///Initialize a new instance.
        /// </summary>
        public RowParser()
        {
            _propertyCache = ParseProperties();
        }

        private static Dictionary<int, PropertyInfo> ParseProperties()
        {
            var cache = new Dictionary<int, PropertyInfo>();

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
                    var otherPropertyName = rowObjectTypeName + "." + otherProperty.Name;
                    var attributeDescription = $"[{columnAttributeName}(\"{columnAttribute.Name}\")]";

                    var errorString = $"Property '{thisPropertyName}' has {attributeDescription}, but '{otherPropertyName}' already claimed that column.";

                    throw new InvalidOperationException(errorString);
                }

                cache[columnIndex] = property;
            }

            if (!cache.Any())
            {
                throw new InvalidOperationException($"No [{columnAttributeName}] found on {rowObjectTypeName}");
            }

            return cache;
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

                var value = cell.Value.ToString();

                property.SetValue(rowData, value);
            }

            return rowData;
        }
    }
}
