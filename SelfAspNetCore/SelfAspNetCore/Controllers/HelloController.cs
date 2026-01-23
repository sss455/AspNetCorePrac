using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers;

public class HelloController : Controller
{
    private readonly MyContext _db;

    // p.62 [Add] コンストラクター：コンテキストを注入
    public HelloController(MyContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return Content("こんにちは、世界！");

        // ↑と同じ意味。特別な理由が無い限り、IActionResultオブジェクトはヘルパー経由で生成すべき。（ヘルパーを提供するContollerを継承すべき）
        // return new ContentResult(){ Content = "こんにちは、世界！"};
    }

    public IActionResult Show()
    {
        // ビュー変数とは、テンプレートに埋め込む値のこと。
        // ビュー変数は、ViewBagオブジェクトのプロパティとして設定する。
        // 「ViewBag.変数名 = 値;」
        ViewBag.Message = "こんにちは、世界！(ViewBag)";

        // ビュー変数の設定には、ViewDataオブジェクトも利用できる。以下は↑と同じ意味。
        ViewData["Message"] = "こんにちは、世界！(ViewData)";

        // ViewメソッドでRazorテンプレートを呼び出す。（ヘルパーメソッドの一種）
        // 引数なしで呼び出した場合には、アクション名に対応するRazorテンプレートを呼び出す。
        //  例）Views/コントローラー名/アクション名.cshtml
        //    → Views/Hello/Show.cshtml
        return View();

        // テンプレート側の記載例：Show.cshtml
        // <p>@ViewBag.Message</p>
    }

    // p.63 [Add] Booksテーブルからすべてのレコードを取得し表示
    public IActionResult List()
    {
      // データベースから取得した結果をテンプレートに渡す
      //var books = _db.Books;
      DbSet<Book> books = _db.Books;
      return View(books);
    }

    // p.200 [Add] ビューコンポーネントの呼び出し
    public IActionResult List2()
    {
      return View();
    }
    
}
