﻿using System.Data;
using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Provides methods for generating Excel workbooks and datasets from collections of objects.
    /// </summary>
    public static class ExcelGenerator
    {
        /// <summary>
        /// Generates an Excel workbook containing one worksheet with the data from a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection.</typeparam>
        /// <param name="dataList">The collection of objects to add to the worksheet.</param>
        /// <param name="printHeaders">Whether to print the column header names above the data.</param>
        /// <param name="startingRow">The row where the data table should start</param>
        /// <param name="startingColumn">The column where the data table should start</param>
        /// <param name="sheetName">The name to give to the new worksheet.</param>
        /// <param name="createTable">Wheter to create a table of the data and create filters and basic styling</param>
        /// <returns>A <see cref="XLWorkbook" /> workbook containing one worksheet with the data from the collection, or an empty workbook if the collection is empty.</returns>
        public static IXLWorkbook GenerateExcel<T>(
            IReadOnlyCollection<T> dataList,
            bool printHeaders = true,
            int startingRow = 1,
            int startingColumn = 1,
            string? sheetName = null,
            bool createTable = false)
        {
            return new XLWorkbook().AddAndPopulateSheet(dataList, printHeaders, startingRow, startingColumn, sheetName, createTable);
        }

        /// <summary>
        /// Converts a collection of objects to a dataset with one table containing the object data.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection.</typeparam>
        /// <param name="list">The collection of objects to convert to a dataset.</param>
        /// <returns>A <see cref="DataSet" /> dataset containing one table with the object data.</returns>
        public static DataSet ToDataSet<T>(IEnumerable<T> list)
        {
            var columns = PropertyParser.ParseWriteProperties<T>();
            var dataTable = new DataTable();

            dataTable.Columns.AddRange(columns.Select(c => new DataColumn(c.Key, Nullable.GetUnderlyingType(c.Value.PropertyInfo.PropertyType) ?? c.Value.PropertyInfo.PropertyType)).ToArray());

            foreach (var item in list)
            {
                var row = dataTable.NewRow();
                foreach (var column in columns)
                {
                    row[column.Key] = column.Value.PropertyInfo.GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
    }
}
