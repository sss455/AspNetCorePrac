using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SelfAspNet.Lib;

namespace SelfAspNet.Controllers;

public class StateController : Controller
{
    private readonly IDistributedCache _cache;

    public StateController(IDistributedCache cache)
    {
        _cache = cache;
    }
    public IActionResult Cookie()
    {
        ViewBag.Email = HttpContext.Request.Cookies["email"];
        return View();
    }

    [HttpPost]
    public IActionResult Cookie(string email)
    {
        HttpContext.Response.Cookies.Append("email", email, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(7),
            HttpOnly = true
        });
        return RedirectToAction("Cookie");
    }

    public IActionResult Session()
    {
        ViewBag.Email = HttpContext.Session.GetString("email");
        return View();
    }

    [HttpPost]
    public IActionResult Session(string email)
    {
        HttpContext.Session.SetString("email", email);
        return RedirectToAction("Session");
    }

    public IActionResult Json()
    {
        var session = HttpContext.Session;
        if (session.Get<Person>("usr") == null)
        {
            session.Set("usr", new Person("山田雫", 18));
        }
        var usr = session.Get<Person>("usr");
        return Content($"{usr?.Name}：{usr?.Age}歳");
    }

    public async Task<IActionResult> CacheSet()
    {
        await _cache.SetStringAsync("Title", "独習ASP.NET Core",
          new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(20))
        );
        return Content("キャッシュを設定しました。");
    }

    public async Task<IActionResult> CacheGet()
    {
        return Content($"Title：{await _cache.GetStringAsync("Title")}");
    }

    public IActionResult ContextAttr()
    {
    return Content(HttpContext.Items["current"]?.ToString() ?? "No Data");
    }
}