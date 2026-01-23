using System;
using Microsoft.AspNetCore.Mvc;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Lib;

// p.201 [Add] Metaビューコンポーネント（<meta>要素を動的に生成する）
public class MetaViewComponent : ViewComponent
{
    private readonly MyContext _db;

    public MetaViewComponent(MyContext db)
    {
        this._db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync(HttpContext context)
    {
        // ルートパラメーターidを取得
        var id = context.GetRouteValue("id")?.ToString();
        // 存在しない場合、Baseビューを採用
        if(id == null)
        {
            return View("Base");
        }

        // id値で書籍情報を検索
        var book = await _db.Books.FindAsync(Int32.Parse(id));
        // 書籍情報が見つからなかった場合、Baseビューを採用
        if(book == null)
        {
            return View("Base");
        }

        // 書籍情報が取得できた場合は、既定のビューを採用
        return View(book);
    }
}
