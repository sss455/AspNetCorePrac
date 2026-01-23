using Microsoft.AspNetCore.Mvc;

namespace SelfAspNet.Areas.Manage.Controllers;

[Area("Manage")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}