using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetEnhancements.Shared.AspNet
{
    public static class ViewContextExtensions
    {
        public static string? GetMenuClassName(this ViewContext viewContext, string pageName)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            if (string.Equals(activePage, pageName, StringComparison.OrdinalIgnoreCase))
            {
                return "active";
            }
            return null;
        }
    }
}
