namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Determine in which column letter(s) the property's data resides.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnAddressAttribute : Attribute
    {
        /// <summary>
        /// The 1-based index of the column.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Determines the column address ("A" = 1, "AA" = 27, ...).
        /// </summary>
        public ExcelColumnAddressAttribute(string address)
        {
            Index = ColumnExtensions.LetterToIndex(address);
        }

        /// <summary>
        /// Determines the 1-based column's index.
        /// </summary>
        public ExcelColumnAddressAttribute(int index)
        {
            Index = index;
        }
    }
}
