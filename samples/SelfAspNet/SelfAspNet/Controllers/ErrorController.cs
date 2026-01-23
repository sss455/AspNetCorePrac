using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SelfAspNet.Controllers;
public class ErrorController : Controller
{
    public IActionResult Throw()
    {
        throw new Exception();
        // throw new FileNotFoundException();
    }

    public IActionResult Catch(int id)
    {
        return Content($"{id}：ページを正しく表示できません...");
    }
}