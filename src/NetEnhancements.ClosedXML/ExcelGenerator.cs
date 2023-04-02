﻿using ClosedXML.Excel;
using System.Data;

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
        /// <returns>A <see cref="XLWorkbook" /> workbook containing one worksheet with the data from the collection, or an empty workbook if the collection is empty.</returns>
        public static XLWorkbook GenerateExcel<T>(IEnumerable<T> dataList)
            where T : class, new()
        {
            if (!dataList.Any())
            {
                return new XLWorkbook();
            }

            var dataSet = ToDataSet(dataList);

            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(dataSet);
            return workbook;
        }

        /// <summary>
        /// Converts a collection of objects to a dataset with one table containing the object data.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection.</typeparam>
        /// <param name="list">The collection of objects to convert to a dataset.</param>
        /// <returns>A <see cref="DataSet" /> dataset containing one table with the object data.</returns>
        public static DataSet ToDataSet<T>(IEnumerable<T> list)
            where T : class, new()
        {
            var columns = PropertyParser.ParsePropertiesToColumnNames<T>();
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(columns.Select(c => new DataColumn(c.Key, Nullable.GetUnderlyingType(c.Value.PropertyType) ?? c.Value.PropertyType)).ToArray());

            foreach (var item in list)
            {
                var row = dataTable.NewRow();
                foreach (var column in columns)
                {
                    row[column.Key] = column.Value.GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
    }
}