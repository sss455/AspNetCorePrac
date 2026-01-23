// p.472 [Add] アプリの構成
using Microsoft.AspNetCore.Mvc;

namespace SelfAspNetCore.Controllers;

public class ConfigController : Controller
{
    private readonly IConfiguration _config;


    public ConfigController(IConfiguration config)
    {
        _config = config;
    }


    public IActionResult Basic()
    {
        // p.473 [Add] 構成ファイルの値を取得する
        ViewBag.Published1 = _config["MyAppOptions:Published"];
        ViewBag.Projects1  = _config["MyAppOptions:Projects:0"];

        // p.474 [Add] 型厳密な構成情報の取得
        ViewBag.Published2 = _config.GetValue<DateTime>("MyAppOptions:Published");
        ViewBag.Projects2  = _config.GetValue<string>("MyAppOptions:Projects:0");
        return View();
    }
}

