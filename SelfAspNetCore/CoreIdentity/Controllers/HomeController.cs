using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreIdentity.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index()
    {
        return View();
    }


    // p.422 [Add] アクションに認証近状を付与するーーAuthorize属性
    // [Authorize]
    // p.430 [Mod] アクションをロールで制限する
    [Authorize(Roles ="Admin")]
    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
