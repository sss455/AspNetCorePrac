using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNet.Filters;

public class MyAppFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("【MyAppFilter】アクション実行前");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("【MyAppFilter】アクション実行後");
    }
}