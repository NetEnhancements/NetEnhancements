namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom column name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnNameAttribute : Attribute
    {
        /// <summary>
        /// The name of the column.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gives the property a custom column name when writing to Excel.
        /// </summary>
        /// <param name="columnName"></param>
        public ExcelColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
