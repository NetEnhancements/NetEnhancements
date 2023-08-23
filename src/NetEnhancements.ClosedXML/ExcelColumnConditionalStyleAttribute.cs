namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom style name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelColumnConditionalStyleAttribute : ExcelColumnStyleAttribute
    {
        /// <summary>
        /// The condition value the cell's value should meet before applying the conditional styling.
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// The condition logic the cell's value should meet before applying the conditional styling.
        /// </summary>
        public required Condition Condition { get; set; }
    }

    /// <summary>
    /// Specifies the conditions for when to apply the conditional styling.
    /// </summary>
    public enum Condition
    {
        /// <summary>
        /// Apply conditional styling when the value is blank.
        /// </summary>
        WhenIsBlank,

        /// <summary>
        /// Apply conditional styling when the value is not blank.
        /// </summary>
        WhenNotBlank,

        /// <summary>
        /// Apply conditional styling when there's an error.
        /// </summary>
        WhenIsError,

        /// <summary>
        /// Apply conditional styling when there's no error.
        /// </summary>
        WhenNotError,

        /// <summary>
        /// Apply conditional styling when the value contains a specified substring.
        /// </summary>
        WhenContains,

        /// <summary>
        /// Apply conditional styling when the value doesn't contain a specified substring.
        /// </summary>
        WhenNotContains,

        /// <summary>
        /// Apply conditional styling when the value starts with a specified substring.
        /// </summary>
        WhenStartsWith,

        /// <summary>
        /// Apply conditional styling when the value ends with a specified substring.
        /// </summary>
        WhenEndsWith,

        /// <summary>
        /// Apply conditional styling when the value is equal to a specified value.
        /// </summary>
        WhenEquals,

        /// <summary>
        /// Apply conditional styling when the value is not equal to a specified value.
        /// </summary>
        WhenNotEquals,

        /// <summary>
        /// Apply conditional styling when the value is greater than a specified value.
        /// </summary>
        WhenGreaterThan,

        /// <summary>
        /// Apply conditional styling when the value is less than a specified value.
        /// </summary>
        WhenLessThan,

        /// <summary>
        /// Apply conditional styling when the value is equal to or greater than a specified value.
        /// </summary>
        WhenEqualOrGreaterThan,

        /// <summary>
        /// Apply conditional styling when the value is equal to or less than a specified value.
        /// </summary>
        WhenEqualOrLessThan,

        /// <summary>
        /// Apply conditional styling when the value is true.
        /// </summary>
        WhenIsTrue
    }

}
