using System;
using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;

using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

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
        /// <returns>A <see cref="XLWorkbook" /> workbook with the new worksheet added.</returns>
        public static XLWorkbook AddSheet<T>(this XLWorkbook workbook, IEnumerable<T> dataList, string sheetName = "")
            where T : class, new()
        {
            var dataSet = ExcelGenerator.ToDataSet(dataList);
            workbook.Worksheets.Add(dataSet);
            return workbook;
        }

        /// <summary>
        /// Saves the workbook as a byte array in the Excel file format.
        /// </summary>
        /// <param name="workbook">The workbook to save.</param>
        /// <returns>A <see cref="byte"/> array representing the Excel file.</returns>
        public static byte[] ToBytes(this XLWorkbook workbook)
        {
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }


        public static IXLWorksheet InsertDataInternal<T>(this IXLWorksheet work, IEnumerable<T> d)
            where T : class, new()
        {
            var columns = PropertyParser.ParseWriteProperties<T>();

            var currentRowNumber = 1;
            var currentColumnNumber = 1;
            var maximumColumnNumber = currentColumnNumber;
            var maximumRowNumber = currentRowNumber;

            maximumColumnNumber += d.Count() - 1;
            maximumRowNumber += columns.Count - 1;

            // Inline functions to handle looping with transposing
            //////////////////////////////////////////////////////
            void incrementFieldPosition()
            {

                maximumColumnNumber = Math.Max(maximumColumnNumber, currentColumnNumber);
                currentColumnNumber++;
            }

            void incrementRecordPosition()
            {
                maximumRowNumber = Math.Max(maximumRowNumber, currentRowNumber);
                currentRowNumber++;
            }

            void resetRecordPosition()
            {
                currentColumnNumber = 1;
            }

            // Set Headers
            foreach (var propertyTypeInfo in columns)
            {
                var propertyName = propertyTypeInfo.Key;
                work.Cell(currentRowNumber, currentColumnNumber).Value = propertyName;
                incrementFieldPosition();
            }

            incrementRecordPosition();

            // Set Values
            foreach (var item in d)
            {
                resetRecordPosition();
                foreach (var column in columns)
                {
                    var value = column.Value.PropertyInfo.GetValue(item) ?? DBNull.Value;

                    var cell = work.Cell(currentRowNumber, currentColumnNumber);
                    cell.Value = value.ToString();
                    if (!string.IsNullOrEmpty(column.Value.NumberFormat))
                    {
                        cell.Style.NumberFormat.Format = column.Value.NumberFormat;
                    }

                    cell.Style.Alignment.Horizontal = column.Value.HorizontalAlignment;


                    incrementFieldPosition();
                }
                incrementRecordPosition();
            }

            var range = work.Range(
                1,
                1,
                maximumRowNumber,
                maximumColumnNumber);

            range.CreateTable("DT");
            return work;
        }
    }
}
