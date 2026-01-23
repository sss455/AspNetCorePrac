namespace SelfAspNet.Lib;

public class TimeLimitRouteConstraint : IRouteConstraint
{
    public DateTime Begin { get; init; }
    public DateTime End { get; init; }
    public TimeLimitRouteConstraint(string begin, string end)
    {
        DateTime.TryParse(begin, out var b);
        DateTime.TryParse(end, out var e);
        if (b >= e) throw new ArgumentException("開始日＜終了日で指定してください。");
        this.Begin = b;
        this.End = e;
    }
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
      RouteValueDictionary values, RouteDirection routeDirection)
    {
        var current = DateTime.Now;
        return !(current < Begin || current > End);
    }
}