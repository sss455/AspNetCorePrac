using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNet.Filters;

public class MyControllerFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("【MyControllerFilter】アクション実行前");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("【MyControllerFilter】アクション実行後");
    }
}