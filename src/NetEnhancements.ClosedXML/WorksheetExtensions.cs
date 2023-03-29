using ClosedXML.Excel;

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

        public static XLWorkbook AddSheet<T>(this XLWorkbook workbook, IEnumerable<T> dataList, string sheetName = "")
            where T : class, new()
        {
            var dataSet = ExcelGenerator.ToDataSet(dataList);
            workbook.Worksheets.Add(dataSet);
            workbook.Worksheets.Last().Name = sheetName;
            return workbook;
        }

        public static byte[] ToBytes(this XLWorkbook workbook)
        {
            var stream = new MemoryStream();
            workbook.SaveAs(stream);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream.ToArray();
        }
    }
}
