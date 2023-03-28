using ClosedXML.Excel;

namespace NetEnhancements.ClosedXML
{
    /// <summary>
    /// Extension method container.
    /// </summary>
    public static class CellExtensions
    {
        /// <summary>
        /// Read a cell's value, returning a default value when unable to.
        /// </summary>
        public static TValue? GetNullableValue<TValue>(this IXLCell cell, TValue? defaultValue = default)
            where TValue : IComparable<TValue>
        {
            return cell.TryGetValue(out TValue value) ? value ?? defaultValue : defaultValue;
        }
    }
}