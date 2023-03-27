using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetEnhancements.AspNet
{
    public static class SelectListExtensions
    {
        /// <summary>
        /// Convert any list to a select list.
        /// </summary>
        public static ICollection<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> items,
            Func<T, string> textSelector,
            Func<T, string> valueSelector,
            string? selectedValue = null
        )
        {
            return items.Select(i =>
                {
                    var item = new SelectListItem(textSelector(i), valueSelector(i));

                    item.Selected = item.Value == selectedValue;

                    return item;
                })
                .ToList()
                .AsReadOnly();
        }
    }
}
