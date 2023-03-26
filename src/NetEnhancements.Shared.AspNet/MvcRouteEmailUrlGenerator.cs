//using NetEnhancements.Email;

namespace NetEnhancements.Shared.AspNet
{
    //public class MvcRouteEmailUrlGenerator : IUrlGenerator
    //{
    //    private readonly HostString _hostname;
    //    private readonly LinkGenerator _linkGenerator;

    //    public MvcRouteEmailUrlGenerator(IOptions<BusinessSettings> mailSettings, LinkGenerator linkGenerator)
    //    {
    //        _hostname = new HostString(mailSettings.Value.Hostname);
    //        _linkGenerator = linkGenerator;
    //    }

    //    public string AreaPage(string pageName, object routeValuesIncludingArea)
    //    {
    //        var absoluteUrl = _linkGenerator.GetUriByPage(pageName, null, routeValuesIncludingArea, "https", _hostname);

    //        return absoluteUrl ?? throw new ArgumentException($"Cannot generate link for page '{pageName}' with the provided route values", nameof(pageName));
    //    }
    //}
}
