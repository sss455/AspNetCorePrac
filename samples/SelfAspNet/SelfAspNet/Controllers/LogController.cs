using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SelfAspNet.Lib;

namespace SelfAspNet.Controllers;

public class LogController : Controller
{
    private readonly ILogger _logger;

    public LogController(ILogger<LogController> logger)
    {
        _logger = logger;
    }

    // public LogController(ILoggerFactory factory)
    // {
    //     _logger = factory.CreateLogger("MyApp");
    // }

    public IActionResult Basic()
    {
        _logger.LogTrace("トレース");
        _logger.LogDebug("デバッグ");
        _logger.LogInformation("情報");
        _logger.LogWarning("警告");
        _logger.LogError("エラー");
        _logger.LogCritical("致命的な問題");
        // _logger.Log(LogLevel.Critical, "致命的な問題");
        return Content("ログはコンソールなどから確認してください。");
    }

    public IActionResult Scope()
    {
        using (_logger.BeginScope("トップ"))
        {
            _logger.LogWarning("処理を開始しました。");
            for (int i = 0; i < 3; i++)
            {
                using (_logger.BeginScope("Loop {Index}", i))
                {
                    _logger.LogWarning("ナノ秒：{Current}", DateTime.Now.Nanosecond);
                }
            }
        }
        return Content("ログはコンソールなどから確認してください。");
    }

    public IActionResult Message()
    {
        _logger.LogWarning("{Path} -> {Current: yyyy年MM月dd日}",
          Request.Path, DateTime.Now);
        return Content("ログはコンソールなどから確認してください。");
    }

    public IActionResult Event()
    {
        _logger.LogWarning(MyAppEvents.CreateData, "致命的な問題");
        return Content("ログはコンソールなどから確認してください。");
    }
}