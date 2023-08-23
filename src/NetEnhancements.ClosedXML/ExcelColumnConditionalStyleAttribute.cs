namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom style name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelColumnConditionalStyleAttribute : ExcelColumnStyleAttribute
    {
        public string Value { get; set; }

        public Condition Condition { get; set; }
    }

    public enum Condition
    {
        WhenIsBlank,
        WhenNotBlank,
        WhenIsError,
        WhenNotError,
        WhenContains,
        WhenNotContains,
        WhenStartsWith,
        WhenEndsWith,
        WhenEquals,
        WhenNotEquals,
        WhenGreaterThan,
        WhenLessThan,
        WhenEqualOrGreaterThan,
        WhenEqualOrLessThan,
        WhenIsTrue
    }
}
