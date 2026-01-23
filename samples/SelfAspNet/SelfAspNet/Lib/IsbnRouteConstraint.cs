using System.Text.RegularExpressions;

namespace SelfAspNet.Lib;

public class IsbnRouteConstraint : IRouteConstraint
{
    private readonly bool _is13SDigits = true;

    private readonly Regex _isbn13 = new("^978-4-[0-9]{2,5}-[0-9]{2,5}-[0-9X]$"
      , RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private readonly Regex _isbn10 = new("^4-[0-9]{2,5}-[0-9]{2,5}-[0-9X]$"
      , RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public IsbnRouteConstraint() : this(true) { }

    public IsbnRouteConstraint(bool is13Digits)
    {
        _is13SDigits = is13Digits;
    }

    public bool Match(HttpContext? httpContext, IRouter? route,
      string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value)
          && value != null)
        {
            var strValue = Convert.ToString(value)!;
            return _is13SDigits ?
              strValue.Length == 17 && _isbn13.IsMatch(strValue) :
              strValue.Length == 13 && _isbn10.IsMatch(strValue);
        }
        return false;
    }
}