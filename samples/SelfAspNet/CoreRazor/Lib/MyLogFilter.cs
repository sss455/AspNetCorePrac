using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreRazor.Lib;
public class MyLogFilter : IPageFilter
{
    private readonly IConfiguration _config;
    public MyLogFilter(IConfiguration config)
    {
        _config = config;
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        Console.WriteLine($"OnPageHandlerExecuted: {context.ActionDescriptor.DisplayName}");
    }
    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        Console.WriteLine($"OnPageHandlerExecuting: {context.ActionDescriptor.DisplayName}");
    }
    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        Console.WriteLine($"OnPageHandlerSelected: {context.ActionDescriptor.DisplayName}");
    }
}