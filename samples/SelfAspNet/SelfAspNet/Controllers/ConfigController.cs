using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SelfAspNet.Lib;

namespace SelfAspNet.Controllers;

public class ConfigController : Controller
{
    private readonly IConfiguration _config;
    private readonly MyAppOptions _app;
    private readonly ApiInfoOptions _slide;
    private readonly ApiInfoOptions _weather;


    // public ConfigController(IConfiguration config)
    // {
    //     _config = config;
    // }

    public ConfigController(IConfiguration config, IOptions<MyAppOptions> app,
        IOptionsSnapshot<ApiInfoOptions> api)
    {
        _config = config;
        _app = app.Value;
        _slide = api.Get(ApiInfoOptions.SlideShow);
        _weather = api.Get(ApiInfoOptions.OpenWeather);
    }

    public IActionResult Basic()
    {
        ViewBag.Published = _config["MyAppOptions:Published"];
        ViewBag.Projects = _config["MyAppOptions:Projects:0"];

        // ViewBag.Published = _config.GetValue<DateTime>("MyAppOptions:Published");
        // ViewBag.Projects = _config.GetValue<string>("MyAppOptions:Projects:0");
        return View();
    }

    public IActionResult Typed()
    {
        ViewBag.Published = _app.Published.ToLongDateString();
        ViewBag.Projects = _app.Projects[0];
        return View("Basic");
    }

    public IActionResult Named()
    {
        return Content(@$"
            SlideShow API：{_slide.BaseUrl}
            OpenWeather API：{_weather.BaseUrl}
        ");
    }

    public IActionResult Args()
    {
        return Content(@$"
            OpenWeather API Key：{_config.GetValue<string>("OpenWeather:ApiKey")}
        ");
    }

    public IActionResult Env()
    {
        return Content(@$"
            OpenWeather API Key：{_config.GetValue<string>("OpenWeather:ApiKey")}
            ENVIRONMENT：{_config.GetValue<string>("ENVIRONMENT")}
        ");
    }

    public IActionResult Xml()
    {
        return Content(@$"
            OpenWeather API Key：{_config.GetValue<string>("OpenWeather:ApiKey")}
        ");
    }

    public IActionResult Memory()
    {
        return Content(@$"
            Company：{_config.GetValue<string>("Company")}
            WINGS-DM:受信：{_config.GetValue<string>("WINGS-DM:Accept")}
            WINGS-DM:送信時刻：{_config.GetValue<string>("WINGS-DM:SendTime")}
            既定のログレベル：{_config.GetValue<string>("Logging:LogLevel:Default")}
        ");
    }

    public IActionResult Secret()
    {
        return Content(@$"
            OpenWeather API Key：{_config.GetValue<string>("OpenWeather:ApiKey")}
        ");
    }
}