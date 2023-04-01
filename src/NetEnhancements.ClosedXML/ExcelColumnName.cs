namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom column name when writing to Excel.
    /// </summary>
    public class ExcelColumnName : Attribute
    {
        public readonly string ColumnName;

        /// <summary>
        /// Gives the property a custom column name when writing to Excel.
        /// </summary>
        /// <param name="columnName"></param>
        public ExcelColumnName(string columnName)
        {
            this.ColumnName = columnName;
        }
    }
}
