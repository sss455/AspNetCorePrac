using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace SelfAspNet.Controllers;

public class I18nController : Controller
{
    private readonly IStringLocalizer<I18nController> _localizer;
    private readonly IHtmlLocalizer<I18nController> _htmlLocalizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

    public I18nController(
        IStringLocalizer<I18nController> loc,
        IHtmlLocalizer<I18nController> htmlLoc,
        IStringLocalizer<SharedResource> sharedLoc)
    {
        _localizer = loc;
        _htmlLocalizer = htmlLoc;
        _sharedLocalizer = sharedLoc;
    }

    public IActionResult Basic()
    {
        return View();
    }

    public IActionResult Ctrl()
    {
        ViewBag.Night = _localizer["Night"];
        ViewBag.Time = _htmlLocalizer["Time", DateTime.Now.ToShortTimeString()];
        return View();
    }

    public IActionResult Global()
    {
        ViewBag.Common = _sharedLocalizer["Common"];
        return View();
    }

    public IActionResult Template()
    {
        return View();
    }
}