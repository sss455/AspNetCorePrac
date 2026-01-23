using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreRazor.Pages.Filter;

public class InlineModel : PageModel
{
    public string Message { get; set; } = string.Empty;
    private readonly IConfiguration _config;
    public InlineModel(IConfiguration config)
    {
        _config = config;
    }

    public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        Console.WriteLine($"OnPageHandlerExecuted: {context.ActionDescriptor.DisplayName}");
    }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        Console.WriteLine($"OnPageHandlerExecuting: {context.ActionDescriptor.DisplayName}");
    }
    public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        Console.WriteLine($"OnPageHandlerSelected: {context.ActionDescriptor.DisplayName}");
    }

    public void OnGet()
    {
        Message = "こんにちは、世界！";
    }
}
