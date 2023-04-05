namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Extension method container.
    /// </summary>
    public static class ColumnExtensions
    {
        /// <summary>
        /// Returns the column index from its name ("A" = 0, "AA" = 26, ...).
        /// </summary>
        /// <devdoc>
        /// Not an extension method on purpose, to not pollute the string type.
        /// </devdoc>
        public static int LetterToIndex(string columnName)
        {
            int index = 0;
            
            columnName = columnName.ToUpper();

            foreach (char c in columnName)
            {
                if (c is < 'A' or > 'Z')
                {
                    throw new ArgumentException("Column name can only contain A-Z", nameof(columnName));
                }

                index = (index * 26) + (c - 64);
            }

            return index;
        }
    }
}
