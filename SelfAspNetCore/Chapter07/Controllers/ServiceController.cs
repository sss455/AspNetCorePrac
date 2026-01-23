using Chapter07.Lib;
using Microsoft.AspNetCore.Mvc;

namespace Chapter07.Controllers;

public class ServiceController : Controller
{
    // p.450 [Add] サービスのスコープ（有効期間）
    private readonly ISingletonService _singletonSvc1;
    private readonly ISingletonService _singletonSvc2;
    private readonly IScopedService    _scopedSvc1;
    private readonly IScopedService    _scopedSvc2;
    private readonly ITransientService _transientSvc1;
    private readonly ITransientService _transientSvc2;

    // p.454 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
    private readonly IMessageService _msg;
    private readonly IEnumerable<IMessageService> _multi;


    // コンストラクター
    public ServiceController(
                    // p.450 [Add] サービスのスコープ（有効期間）
                    ISingletonService singletonSvc1, ISingletonService singletonSvc2,
                    IScopedService    scopedSvc1,    IScopedService    scopedSvc2,
                    ITransientService transientSvc1, ITransientService transientSvc2,
                    // p.454 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
                    // IMssageService(単一)を注入
                    IMessageService msg,
                    // IMssageService(複数)を注入
                    // ※IEnumerable<TService>の形式でサービスを注入(この場合、サービス型に紐づくすべての実装型が注入される)
                    IEnumerable<IMessageService> multi)
    {
        // p.450 [Add] サービスのスコープ（有効期間）
        _singletonSvc1 = singletonSvc1;
        _singletonSvc2 = singletonSvc2;
        _scopedSvc1    = scopedSvc1;
        _scopedSvc2    = scopedSvc2;
        _transientSvc1 = transientSvc1;
        _transientSvc2 = transientSvc2;
        // p.454 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
        _msg = msg;
        _multi = multi;
    }

    // p.450 [Add] サービスのスコープ（有効期間）
    public IActionResult Scope()
    {
        ViewBag.SingletonSvc1 = _singletonSvc1.Id.ToString();
        ViewBag.SingletonSvc2 = _singletonSvc2.Id.ToString();
        
        ViewBag.ScopedSvc1    = _scopedSvc1.Id.ToString();
        ViewBag.ScopedSvc2    = _scopedSvc2.Id.ToString();
        
        ViewBag.TransientSvc1 = _transientSvc1.Id.ToString();
        ViewBag.TransientSvc2 = _transientSvc2.Id.ToString();

        return View();
        //return Content($"{_svc.Id.ToString()} / {_svc2.Id.ToString()}");
    }

    // p.454 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
    public IActionResult Multi()
    {
        // ※この場合、あとに登録されたサービスが優先される。
        return Content(_msg.Message);
    }

    // p.455 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
    // 複数サービスからのメッセージを結合＆表示
    public IActionResult Multi2()
    {
        var list = new List<string>();
        foreach(var msg in _multi) list.Add(msg.Message);

        return Content(string.Join(", ", list));
    }
    public IActionResult Multi3()
    {
        var list = _multi.ToList<IMessageService>();
        return Content($"{list[0].Message} / {list[1].Message}");
    }
}