using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfAspNet.Filters;

namespace CoreRazor.Pages.Filter;

[TimeLimit("2023/12/01", "2024/12/31")]
public class IndexModel : PageModel
{
    public string Message { get; set; } = "";
    private readonly IConfiguration _config;
    public IndexModel(IConfiguration config)
    {
        _config = config;
    }

    public void OnGet()
    {
        Message = "こんにちは、世界！";
    }
}
