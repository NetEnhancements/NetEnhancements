namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Extension method container.
    /// </summary>
    public static class ColumnExtensions
    {
        /// <summary>
        /// Returns the column index from its name ("A" = 1, "AA" = 27, ...).
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

        /// <summary>
        /// Returns the column letter from its index (1 = "A", 27 = "AA", ...).
        /// </summary>
        public static string IndexToLetter(int columnIndex)
        {
            string result = "";

            while (columnIndex > 0)
            {
                int remainder = columnIndex % 26;
                result = (char)('A' + remainder) + result;
                columnIndex = (columnIndex / 26);
            }

            return result;
        }
    }
}
