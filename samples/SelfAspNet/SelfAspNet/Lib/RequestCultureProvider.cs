using Microsoft.AspNetCore.Localization;

namespace SelfAspNet.Lib;

public class RouteValueRequestCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(
      HttpContext httpContext)
    {
        var routes = httpContext.Request.RouteValues;
        if (routes == null) return NullProviderCultureResult;

        var culture = (string?)routes["culture"];
        if (culture == null) return NullProviderCultureResult;

        return Task.FromResult<ProviderCultureResult?>(
          new ProviderCultureResult(culture));
    }
}
