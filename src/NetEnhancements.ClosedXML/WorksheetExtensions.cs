using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Provides extension methods for working with Excel worksheets and workbooks.
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
        /// Adds a new worksheet to the workbook containing the data from a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection.</typeparam>
        /// <param name="workbook">The workbook to add the worksheet to.</param>
        /// <param name="dataList">The collection of objects to add to the worksheet.</param>
        /// <param name="sheetName">The name to give to the new worksheet.</param>
        /// <returns>The workbook with the new worksheet added.</returns>
        public static XLWorkbook AddSheet<T>(this XLWorkbook workbook, IEnumerable<T> dataList, string sheetName = "")
            where T : class, new()
        {
            var dataSet = ExcelGenerator.ToDataSet(dataList);
            workbook.Worksheets.Add(dataSet);
            workbook.Worksheets.Last().Name = sheetName;
            return workbook;
        }

        /// <summary>
        /// Saves the workbook as a byte array in the Excel file format.
        /// </summary>
        /// <param name="workbook">The workbook to save.</param>
        /// <returns>A byte array representing the Excel file.</returns>
        public static byte[] ToBytes(this XLWorkbook workbook)
        {
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
