namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Gives the property a custom style name when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnConditionalStyleAttribute : ExcelColumnStyleAttribute
    {
        public string Value { get; set; }

        public Conditions Condition { get; set; }
    }

    public enum Conditions
    {
        WhenIsBlank
        , WhenNotBlank
        , WhenIsError
        , WhenNotError
        , WhenContains
        , WhenNotContains
        , WhenStartsWith
        , WhenEndsWith
        , WhenEquals
        , WhenNotEquals
        , WhenGreaterThan
        , WhenLessThan
        , WhenEqualOrGreaterThan
        , WhenEqualOrLessThan
        , WhenIsTrue
    }
}
