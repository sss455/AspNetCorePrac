using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNet.Filters;

public class MyActionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("【MyActionFilter】アクション実行前");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("【MyActionFilter】アクション実行後");
    }
}