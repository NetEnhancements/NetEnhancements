﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

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

            if (createTable)
            {
                var range = sheet.Range(startingRow, startingColumn, maximumRowNumber, maximumColumnNumber - 1);

                range.CreateTable();
            }
        }

        private static void SetCellStyle(IXLCell cell, KeyValuePair<string, WritePropertyTypeInfo> column)
        {
            if (column.Value.ExcelColumnStyle != null)
            {
                cell.Style.AddStyle(column.Value.ExcelColumnStyle);
            }

            if (column.Value.ExcelColumnConditionalStyle != null)
            {
                var conditionalFormat = cell.AddConditionalFormat();
                switch (column.Value.ExcelColumnConditionalStyle.Condition)
                {
                    case Condition.WhenIsBlank:
                        conditionalFormat.WhenIsBlank().AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenNotBlank:
                        conditionalFormat.WhenNotBlank().AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenIsError:
                        conditionalFormat.WhenIsError().AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenNotError:
                        conditionalFormat.WhenNotError().AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenContains:
                        conditionalFormat.WhenContains(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenNotContains:
                        conditionalFormat.WhenNotContains(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenStartsWith:
                        conditionalFormat.WhenStartsWith(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenEndsWith:
                        conditionalFormat.WhenEndsWith(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenEquals:
                        conditionalFormat.WhenEquals(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenNotEquals:
                        conditionalFormat.WhenNotEquals(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenGreaterThan:
                        conditionalFormat.WhenGreaterThan(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenLessThan:
                        conditionalFormat.WhenLessThan(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenEqualOrGreaterThan:
                        conditionalFormat.WhenEqualOrGreaterThan(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenEqualOrLessThan:
                        conditionalFormat.WhenEqualOrLessThan(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                    case Condition.WhenIsTrue:
                        conditionalFormat.WhenIsTrue(column.Value.ExcelColumnConditionalStyle.Value).AddStyle(column.Value.ExcelColumnConditionalStyle);
                        break;
                }
            }
        }

        private static void AddStyle(this IXLStyle style, ExcelColumnStyleAttribute column)
        {
            if (!string.IsNullOrEmpty(column.NumberFormat))
            {
                style.NumberFormat.Format = column.NumberFormat;
            }

            if (!string.IsNullOrEmpty(column.DateFormat))
            {
                style.DateFormat.Format = column.DateFormat;
            }

            if (column.IsHorizontalAlignmentSet)
            {
                style.Alignment.Horizontal = column.HorizontalAlignment;
            }

            if (column.IsVerticalAlignmentSet)
            {
                style.Alignment.Vertical = column.VerticalAlignment;
            }

            if (column.IsTopBorderSet)
            {
                style.Border.TopBorder = column.TopBorder;
            }
            if (column.IsBottomBorderSet)
            {
                style.Border.BottomBorder = column.BottomBorder;
            }
            if (column.IsLeftBorderSet)
            {
                style.Border.LeftBorder = column.LeftBorder;
            }
            if (column.IsRightBorderSet)
            {
                style.Border.RightBorder = column.RightBorder;
            }

            if (!string.IsNullOrEmpty(column.TopBorderColor))
            {
                style.Border.TopBorderColor = XLColor.FromHtml(column.TopBorderColor);
            }
            if (!string.IsNullOrEmpty(column.BottomBorderColor))
            {
                style.Border.BottomBorderColor = XLColor.FromHtml(column.BottomBorderColor);
            }
            if (!string.IsNullOrEmpty(column.LeftBorderColor))
            {
                style.Border.LeftBorderColor = XLColor.FromHtml(column.LeftBorderColor);
            }
            if (!string.IsNullOrEmpty(column.RightBorderColor))
            {
                style.Border.RightBorderColor = XLColor.FromHtml(column.RightBorderColor);
            }

            if (!string.IsNullOrEmpty(column.FillColor))
            {
                style.Fill.BackgroundColor = XLColor.FromHtml(column.FillColor);
            }
            if (!string.IsNullOrEmpty(column.FontColor))
            {
                style.Font.FontColor = XLColor.FromHtml(column.FontColor);
            }

            style.Font.Bold = column.FontBold;

            style.IncludeQuotePrefix = column.IncludeQuotePrefix;
            style.SetIncludeQuotePrefix(column.SetIncludeQuotePrefix);
            style.Protection.Locked = column.IsProtected;

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
