using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SelfAspNet.Filters;
using SelfAspNet.Lib;

namespace SelfAspNet.Controllers;

// [MyLog]
// [MyControllerFilter]
// [MyControllerFilter(Order = int.MinValue)]
public class FilterController : Controller
{
    // public override void OnActionExecuting(ActionExecutingContext context)
    // {
    //     Console.WriteLine($"【Before】{context.ActionDescriptor.DisplayName}が実行されます。");
    // }

    // public override void OnActionExecuted(ActionExecutedContext context)
    // {
    //     Console.WriteLine($"【After】{context.ActionDescriptor.DisplayName}が実行されました。");
    // }

    [MyLog]
    // [MyLogAsync]
    public IActionResult Index()
    {
        Console.WriteLine($"【Action】アクション本体です。");
        ViewBag.Message = "こんにちは、世界！";
        return View();
    }

    [MyActionFilter]
    public IActionResult Order()
    {
        Console.WriteLine("Filterアクション実行");
        return Content("Filterアクション実行");
    }

    [TimeLimit("2024/05/01", "2024/07/15")]
    public IActionResult Range()
    {
        return Content("キャンペーン期間中です。");
    }

    [TypeFilter(typeof(LogExceptionFilter))]
    // [TypeFilter(typeof(LogExceptionFilter), Arguments = new[] { "MyValue" })]
    // [ServiceFilter(typeof(LogExceptionFilter))]
    // [LogException]
    public IActionResult Except()
    {
        throw new Exception("問題が発生しました！");
    }

    public IActionResult Csrf()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Process()
    {
        return Content("処理終了");
    }

    [RefererSelector(false)]
    public IActionResult Referer()
    {
        return Content("正しくアクセスできました。");
    }
}