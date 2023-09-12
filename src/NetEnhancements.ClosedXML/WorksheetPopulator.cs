using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// It puts the data into the worksheet.
    /// </summary>
    internal static class WorksheetPopulator
    {
        internal static void Populate<T>(IReadOnlyCollection<T> dataList, IXLWorksheet sheet, bool printHeaders, int startingRow, int startingColumn, bool createTable)
        {
            var columns = PropertyParser.ParseWriteProperties<T>(dataList.First()?.GetType());

            var currentRowNumber = startingRow;
            var currentColumnNumber = startingColumn;
            var maximumColumnNumber = currentColumnNumber;
            var maximumRowNumber = currentRowNumber;

            maximumColumnNumber += columns.Count - 1;
            maximumRowNumber += dataList.Count - 1;

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

                IncrementRecordPosition();
            }

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
                var range = sheet.Range(startingRow, startingColumn, maximumRowNumber, maximumColumnNumber);
                range.CreateTable();
            }
        }

        private static void SetCellStyle(IXLCell cell, KeyValuePair<string, WritePropertyTypeInfo> column)
        {
            if (column.Value.ExcelColumnStyle != null)
            {
                cell.Style.ApplyStyleAttribute(column.Value.ExcelColumnStyle);
            }

            foreach (var cond in column.Value.ConditionalStyleAttributes)
            {
                GetCondition(cond.Condition, cell, cond.Value).ApplyStyleAttribute(cond);
            }
        }

        private static IXLStyle GetCondition(Condition condition, IXLCell cell, string? value)
        {
            var conditionalFormat = cell.AddConditionalFormat();

            return condition switch
            {
                Condition.WhenIsBlank => conditionalFormat.WhenIsBlank(),
                Condition.WhenNotBlank => conditionalFormat.WhenNotBlank(),
                Condition.WhenIsError => conditionalFormat.WhenIsError(),
                Condition.WhenNotError => conditionalFormat.WhenNotError(),
                Condition.WhenContains => conditionalFormat.WhenContains(value),
                Condition.WhenNotContains => conditionalFormat.WhenNotContains(value),
                Condition.WhenStartsWith => conditionalFormat.WhenStartsWith(value),
                Condition.WhenEndsWith => conditionalFormat.WhenEndsWith(value),
                Condition.WhenEquals => conditionalFormat.WhenEquals(value),
                Condition.WhenNotEquals => conditionalFormat.WhenNotEquals(value),
                Condition.WhenGreaterThan => conditionalFormat.WhenGreaterThan(value),
                Condition.WhenLessThan => conditionalFormat.WhenLessThan(value),
                Condition.WhenEqualOrGreaterThan => conditionalFormat.WhenEqualOrGreaterThan(value),
                Condition.WhenEqualOrLessThan => conditionalFormat.WhenEqualOrLessThan(value),
                Condition.WhenIsTrue => conditionalFormat.WhenIsTrue(value),
                _ => throw new ArgumentException($"Unknown condition type {condition}")
            };
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
