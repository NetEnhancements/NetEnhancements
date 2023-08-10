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
        public static void Populate<T>(this IXLWorksheet sheet, IReadOnlyCollection<T> dataList, bool printHeaders, int startingRow = 1, int startingColumn = 1)
        {
            if (!dataList.Any())
            {
                return;
            }

            var columns = PropertyParser.ParseWriteProperties<T>(dataList.First()?.GetType());

            var currentRowNumber = startingRow;
            var currentColumnNumber = startingColumn;
            var maximumColumnNumber = currentColumnNumber;
            var maximumRowNumber = currentRowNumber;

            maximumColumnNumber += dataList.Count - 1;
            maximumRowNumber += columns.Count - 1;

            void IncrementFieldPosition()
            {
                maximumColumnNumber = Math.Max(maximumColumnNumber, currentColumnNumber);
                currentColumnNumber++;
            }

            void IncrementRecordPosition()
            {
                maximumRowNumber = Math.Max(maximumRowNumber, currentRowNumber);
                currentRowNumber++;
            }

            void ResetRecordPosition()
            {
                currentColumnNumber = startingColumn;
            }

            // Set Headers
            if (printHeaders)
            {
                foreach (var (propertyName, _) in columns)
                {
                    sheet.Cell(currentRowNumber, currentColumnNumber).Value = propertyName;
                    IncrementFieldPosition();
                }
            }

            IncrementRecordPosition();

            // Set Values
            foreach (var item in dataList)
            {
                ResetRecordPosition();

                if (item != null)
                {
                    foreach (var column in columns)
                    {
                        var cell = sheet.Cell(currentRowNumber, currentColumnNumber);

                        if (!column.Value.PropertyInfo.DeclaringType!.IsInstanceOfType(item))
                        {
                            throw new InvalidOperationException($"{item.GetType().FullName} is not the same type as {column.Value.PropertyInfo.DeclaringType.FullName}.");
                        }

                        var cellValue = GetCellValue(column.Value.PropertyInfo.GetValue(item));

                        cell.Value = cellValue;

                        SetCellStyle(cell, column);

                        IncrementFieldPosition();
                    }
                }

                IncrementRecordPosition();
            }
        }

        private static void SetCellStyle(IXLCell cell, KeyValuePair<string, WritePropertyTypeInfo> column)
        {
            if (!string.IsNullOrEmpty(column.Value.NumberFormat))
            {
                cell.Style.NumberFormat.Format = column.Value.NumberFormat;
            }

            if (!string.IsNullOrEmpty(column.Value.DateFormat))
            {
                cell.Style.DateFormat.Format = column.Value.DateFormat;
            }

            if (column.Value.HorizontalAlignment.HasValue)
            {
                cell.Style.Alignment.Horizontal = column.Value.HorizontalAlignment.Value;
            }

            if (column.Value.VerticalAlignment.HasValue)
            {
                cell.Style.Alignment.Vertical = column.Value.VerticalAlignment.Value;
            }

            if (column.Value.TopBorder.HasValue)
            {
                cell.Style.Border.TopBorder = column.Value.TopBorder.Value;
            }
            if (column.Value.BottomBorder.HasValue)
            {
                cell.Style.Border.BottomBorder = column.Value.BottomBorder.Value;
            }
            if (column.Value.LeftBorder.HasValue)
            {
                cell.Style.Border.LeftBorder = column.Value.LeftBorder.Value;
            }
            if (column.Value.RightBorder.HasValue)
            {
                cell.Style.Border.RightBorder = column.Value.RightBorder.Value;
            }

            if (!string.IsNullOrEmpty(column.Value.TopBorderColor))
            {
                cell.Style.Border.TopBorderColor = XLColor.FromHtml(column.Value.TopBorderColor);
            }
            if (!string.IsNullOrEmpty(column.Value.BottomBorderColor))
            {
                cell.Style.Border.BottomBorderColor = XLColor.FromHtml(column.Value.BottomBorderColor);
            }
            if (!string.IsNullOrEmpty(column.Value.LeftBorderColor))
            {
                cell.Style.Border.LeftBorderColor = XLColor.FromHtml(column.Value.LeftBorderColor);
            }
            if (!string.IsNullOrEmpty(column.Value.RightBorderColor))
            {
                cell.Style.Border.RightBorderColor = XLColor.FromHtml(column.Value.RightBorderColor);
            }

            if (!string.IsNullOrEmpty(column.Value.FillColor))
            {
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(column.Value.FillColor);
            }
            if (!string.IsNullOrEmpty(column.Value.FontColor))
            {
                cell.Style.Font.FontColor = XLColor.FromHtml(column.Value.FontColor);
            }

            cell.Style.Font.Bold = column.Value.FontBold;

            cell.Style.IncludeQuotePrefix = column.Value.IncludeQuotePrefix;
            cell.Style.SetIncludeQuotePrefix(column.Value.SetIncludeQuotePrefix);
            cell.Style.Protection.Locked = column.Value.IsProtected;
        }

        private static XLCellValue GetCellValue(object? value)
        {
            // Thanks to magic of JIT compiler, generic method is compiled as a separate method
            // for each T, so the whole switch removes types that don't match T and the whole
            // switch is actually reduced only to the code for specific T (=no switch for value types).
            XLCellValue newValue = value switch
            {
                null => Blank.Value,
                Blank blankValue => blankValue,
                Boolean logical => logical,
                SByte number => number,
                Byte number => number,
                Int16 number => number,
                UInt16 number => number,
                Int32 number => number,
                UInt32 number => number,
                Int64 number => number,
                UInt64 number => number,
                Single number => number,
                Double number => number,
                Decimal number => number,
                String text => text,
                XLError error => error,
                DateTime date => date,
                DateOnly date => date.ToDateTime(default),
                DateTimeOffset dateOfs => dateOfs.DateTime,
                TimeSpan timeSpan => timeSpan,
                TimeOnly time => time.ToTimeSpan(),
                _ => value.ToString() // Other things, like chars ect are just turned to string
            };

            return newValue;
        }
    }
}
