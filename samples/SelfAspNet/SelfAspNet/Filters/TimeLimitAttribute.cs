using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNet.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TimeLimitAttribute : Attribute, IAuthorizationFilter
{
    public DateTime Begin { get; init; }
    public DateTime End { get; init; }

    public TimeLimitAttribute(string begin, string end)
    {
        DateTime.TryParse(begin, out var b);
        DateTime.TryParse(end, out var e);
        if (b >= e) throw new ArgumentException("開始日＜終了日で指定してください。");
        Begin = b;
        End = e;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var current = DateTime.Now;
        if (current < Begin || current > End)
        {
            context.Result = new ContentResult()
            {
                Content = $"このページは{Begin.ToShortDateString()}～{End.ToShortDateString()}の期間のみアクセスできます。"
            };
        }
    }
}