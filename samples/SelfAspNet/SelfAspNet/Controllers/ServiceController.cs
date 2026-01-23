using SelfAspNet.Lib;
using Microsoft.AspNetCore.Mvc;

namespace SelfAspNet.Controllers;

public class ServiceController : Controller
{
    private readonly IMyService _svc;
    private readonly IMyService _svc2;
    private readonly IHogeService _hogesvc;
    private readonly IMessageService _msg;
    private readonly IEnumerable<IMessageService> _multi;


    public ServiceController(IMyService svc, IMyService svc2, IMessageService msg, IEnumerable<IMessageService> multi, IHogeService hoge)
    // public ServiceController(IMyService svc, IMyService svc2, IEnumerable<IMessageService> multi)
    {
        _svc = svc;
        _svc2 = svc2;
        _msg = msg;
        _multi = multi;
        _hogesvc = hoge;
    }

    public IActionResult Scope()
    {
        return Content($"{_svc.Id.ToString()} / {_svc2.Id.ToString()}");
    }

    public IActionResult Multi()
    {
        return Content(_msg.Message);
    }

    public IActionResult Multi2()
    {
        var list = new List<string>();
        foreach (var msg in _multi) list.Add(msg.Message);
        return Content(string.Join(", ", list));
    }

    public IActionResult GetHeader()
    {
        return Content(_hogesvc.Value);
    }
}