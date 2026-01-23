using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace SelfAspNet.Lib;

public class RefererSelectorAttribute : ActionMethodSelectorAttribute
{
  public bool AllowNull { get; init; }

  public RefererSelectorAttribute(bool allowNull = true)
  {
    AllowNull = allowNull;
  }
  
  public override bool IsValidForRequest(
    RouteContext routeContext, ActionDescriptor action)
  {
    var request = routeContext.HttpContext.Request;
    var referer = request.Headers.Referer;
    if (referer.Count == 0) return AllowNull;
    return referer[0]!.Contains($"{request.Host.Value}/");
  }
}