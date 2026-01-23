// p.396 [Add] フィルター
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SelfAspNetCore.Filters;

namespace SelfAspNetCore.Controllers;

// p.401 [Add] コントローラー単位でのフィルターの適用
[MyLog]
// p.405 [Add] フィルターの優先順位を設定する（コントローラー単位）
//[MyControllerFilter(Order=int.MinValue)]
public class FilterController : Controller
{
    // MyLogフィルターを属性として適用
    // ※属性なので、クラス名から接尾辞のAttributeを除いた部分を[..]の形式で表記。
    //   (この例の場合、MyLogAttributeクラスなので、[MyLog]となる)
    [MyLog]
    public IActionResult Index()
    {
        Console.WriteLine("【Action】アクション本体です。");
        //ViewBag.Message = "こんにちは、世界！";
        //return View();
        return Content("こんにちは、世界！");
    }

    // p.402 [Add] コントローラー単位でのフィルターの適用【別解】
    // ※ControllerクラスそのものがIActionFilterインターフェイスを実装しているため、
    //   OnActionExecuting／OnActionExecutedメソッドを直接にオーバーライドできる。
    // アクションの実行後
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"【Before】{context.ActionDescriptor.DisplayName}が実行されます。");
        // ※context.ActionDescriptor.DisplayNameプロパティで、「アクションの完全修飾名」を取得
    }
    // アクションの実行前
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine($"【After】{context.ActionDescriptor.DisplayName}が実行されました。");
    }


    // p.407 [Add] フィルターパイプラインのスキップ
    [TimeLimit("2026/01/06", "2026/01/07")]
    public IActionResult Range()
    {
        return Content("キャンペーン期間中です。");
    }


    // p.410 [Add] 依存性注入を伴うフィルター ーーTypeFilter属性
    // 非属性なフィルターを属性として適用するのは「TypeFilter属性」の役割。
    [TypeFilter(typeof(LogExceptionFilter))]
    public IActionResult ExceptTypeFilter()
    {
        throw new Exception("問題が発生しました。");
    }

    // フィルターのコンストラクターに、依存性注入以外のパラメーターを渡すならば、Argumentsプロパティを指定する
    [TypeFilter( typeof(LogExceptionFilter), Arguments=new[] {"MyValue"} )]
    public IActionResult ExceptTypeFilter2()
    {
        throw new Exception("問題が発生しました。");
    }
    //--------------------------------------------------------------------------------------
    // ※フィルター側のコンストラクターが以下のようであれば、引数headerに"MyValue"が引き渡される。
    //   (複数の値を渡すことも可)
    // public LogExceptionFilter(MyContext context, string header)
    // {
    //    ...
    // }
    //--------------------------------------------------------------------------------------


    // p.412 [Add] 依存性注入を伴うフィルター ーーServiceFilter属性
    // 同様に、非属性なフィルターを属性として適用するための属性（該当フィルターを登録済みのサービスから検索）
    [ServiceFilter(typeof(LogExceptionFilter))]
    public IActionResult ExceptServieFilter()
    {
        throw new Exception("問題が発生しました。");
    }


    // p.413 [Add] 依存性注入を伴うフィルター ーーIFilterFactoryインターフェイス
    // 「（Program.csにサービスとしてアプリに登録）⇒ LogExceptionAttribute(Factory) ⇒ 該当フィルターを登録済みのサービスから取得 ⇒ LogExceptionFilter」
    // ↓シンプルに記述できる
    [LogException]
    public IActionResult ExceptIFilterFactory()
    {
        throw new Exception("問題が発生しました。");
    }


    // p.417 [Add] JavaScriptアプリでのCSRF対策
    public IActionResult Csrf()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Process()
    {
        //return Content("処理終了");
        Console.WriteLine("Processアクションを実行しました。");
        return Empty;
    }


    // p.418 [Add] 応答キャッシュを有効にするーーResponseCache属性
    // ※ResponseCache属性は、アクションに対して応答キャッシュを有効にする機能を追加する。
    //   最低限、キャッシュの有効期限(Duration)を設定しておく（単位は秒）
    [ResponseCache(Duration = 60)]
    public IActionResult ResponseCache(int id, string mode)
    {
        ViewBag.Action = "ResponseCache";
        ViewBag.Id     = id;
        ViewBag.Mode   = mode;

        return View();
    }
    // p.420 [Add] 応答キャッシュを有効にするーーResponseCache属性の主なプロパティ(VaryByXxxxx属性)
    // ※（ルートパラメーターだけでなく）クエリ情報も含めてページを識別をしたい場合、
    // 　 VaryByQueryKeysプロパティに、キャッシュ識別のクエリキーを配列で指定する。
    //    [前提]Program.csで「ResponseChachingミドルウェア」を有効に設定しておく。
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "mode" })]
    public IActionResult ResponseCacheQuery(int id, string mode)
    {
        ViewBag.Action = "ResponseCacheQuery";
        ViewBag.Id     = id;
        ViewBag.Mode   = mode;

        return View("ResponseCache");
    }
    // p.422 [Add] 応答キャッシュを有効にするーーResponseCache属性の主なプロパティ(CacheProfileName属性)
    // ※Program.csであらかじめ"MyChace"というプロファイル名のキャッシュポリシーを定義しておく。
    [ResponseCache(Duration = 60, CacheProfileName = "MyCache")]
    public IActionResult ResponseCacheProfile(int id, string mode)
    {
        ViewBag.Action = "ResponseCacheProfile";
        ViewBag.Id     = id;
        ViewBag.Mode   = mode;

        return View("ResponseCache");
    }
}
