namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Marks property as to not be included when writing to Excel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnIgnoredAttribute : Attribute
    {
    }
}
