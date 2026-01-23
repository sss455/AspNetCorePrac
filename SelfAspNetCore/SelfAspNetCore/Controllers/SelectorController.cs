// p.431 [Add] セレクター属性
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Lib;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers;

public class SelectorController : Controller
{
    private readonly MyContext _context;


    // コンストラクター
    public SelectorController(MyContext context)
    {
        // コンテキストの注入
        _context = context;
    }


    // GET: Selector
    public ActionResult Index()
    {
        return View();
    }


    // Selector/Edit/1
    // 編集画面表示：[Edit]リンクから呼び出され、編集フォームを生成
    public async Task<IActionResult> Edit(int? id)
    {
        #region ...中略...
        if (id == null) { return NotFound(); }

        // 引数idをキーにテーブルを検索
        var book = await _context.Books.FindAsync(id);
        if (book == null) { return NotFound(); }

        // テーブルから重複のない出版社名を取得
        var list = _context.Books
                    .Select(b => new { Publisher = b.Publisher } )
                    .Distinct();
        ViewBag.Opts = new SelectList(list, "Publisher", "Publisher");

        // 編集フォームを表示
        return View(book);
        #endregion
    }


    // p.433 [Add] ブラウザー未対応のHTTPメソッドをサポートする *@
    // [前提]Program.csでHttpMethodOverrideMiddlewareミドルウェアを有効に設定 *@
    // ※ブラウザーから送信されたHTTPメソッドを疑似的に別のメソッドに上書きをすることが可能になる *@
    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, 
                                         [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVewsion")] Book book)
    {
        #region ...中略...
        // 隠しフィールドのid値と、ルートパラメータのidとが一致するか
        if (id != book.Id) { return NotFound(); }

        // 検証に成功したらデータベース処理＆リダイレクト
        if (ModelState.IsValid)
        {
            try
            {
                // エンティティの更新ををデータベースに反映
                _context.Update(book);

                // SaveChangesAsyncメソッドでデータベースへ反映（実操作）
                await _context.SaveChangesAsync();
            }
            // 競合が発生した場合の処理
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        // 入力に問題がある場合は編集フォームを再描画
        return View(book);
        #endregion
    }


    // p.433 [Add] 複数のHTTPメソッドをまとめて宣言する
    // 1つのアクションに複数のHTTPメソッドを紐づけるには、AcceptVerbs属性を利用することでスッキリと表現できる。
    [AcceptVerbs("Post", "Put")]
    //
    // ※もちろん、HttpXxxxx属性を列挙しても構わない。 
    // [HttpPost]
    // [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit2(int id, 
                                         [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVewsion")] Book book)
    {
        #region ...中略...
        // 隠しフィールドのid値と、ルートパラメータのidとが一致するか
        if (id != book.Id) { return NotFound(); }

        // 検証に成功したらデータベース処理＆リダイレクト
        if (ModelState.IsValid)
        {
            try
            {
                // エンティティの更新ををデータベースに反映
                _context.Update(book);

                // SaveChangesAsyncメソッドでデータベースへ反映（実操作）
                await _context.SaveChangesAsync();
            }
            // 競合が発生した場合の処理
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        // 入力に問題がある場合は編集フォームを再描画
        return View(book);
        #endregion
    }

    // p.434 [Add] アクションメソッドを無効化するーーNonAction属性
    // コントローラークラスでは、すべてのPublicな非staticメソッドをアクションと見なす。
    // しかし、Publicではあっても、アクションとしては使われたくないという場合（コントローラー共通で利用するヘルパーであるなど）
    // 対象のメソッドに「NonAction属性」を付与することでアクションとして認識されなくなる。
    [NonAction]
    public IActionResult Privacy()
    {
        return Content("Privacy");
    }


    [RefererSelector(false)]
    public IActionResult Referer()
    {
        return Content("正しくアクセスできました。");
    }
}

