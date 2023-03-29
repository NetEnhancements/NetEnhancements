using ClosedXML.Excel;
using System.Data;

namespace NetEnhancements.ClosedXML
{
    public static class ExcelGenerator
    {
        public static XLWorkbook GenerateExcel<T>(IEnumerable<T> dataList)
            where T : class, new()
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if (!dataList.Any())
            {
                return new XLWorkbook();
            }

            var dataSet = ToDataSet(dataList);

            var wb = new XLWorkbook();
            wb.Worksheets.Add(dataSet);
            return wb;
        }

        public static DataSet ToDataSet<T>(IEnumerable<T> list)
            where T : class, new()
        {
            var properties = PropertyParser.ParsePropertiesToColumnNames<T>();
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);

            // Add a column to table for each public property on T
            foreach (var propInfo in properties)
            {
                var colType = Nullable.GetUnderlyingType(propInfo.Value.PropertyType) ?? propInfo.Value.PropertyType;
                t.Columns.Add(propInfo.Key, colType);
            }

            //go through each property on T and add each value to the table
            foreach (var item in list)
            {
                var row = t.NewRow();

                foreach (var propInfo in properties)
                {
                    row[propInfo.Key] = propInfo.Value.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
    }
}
