using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Provides extension methods for working with Excel worksheets.
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

        /// <summary>
        /// Populates a worksheet with the data from a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection.</typeparam>
        /// <param name="sheet">The workbook to add the worksheet to.</param>
        /// <param name="dataList">The collection of objects to add to the worksheet.</param>
        /// <param name="printHeaders">Whether to print the column header names above the data.</param>
        /// <param name="startingRow">The row where the data table should start</param>
        /// <param name="startingColumn">The column where the data table should start</param>
        /// <param name="createTable">Wheter to create a table of the data and create filters and basic styling</param>
        public static void Populate<T>(this IXLWorksheet sheet, IReadOnlyCollection<T> dataList, bool printHeaders, int startingRow = 1, int startingColumn = 1, bool createTable = false)
        {
            if (!dataList.Any())
            {
                return;
            }

            WorksheetPopulator.Populate(dataList, sheet, printHeaders, startingRow, startingColumn, createTable);
        }
    }
}
