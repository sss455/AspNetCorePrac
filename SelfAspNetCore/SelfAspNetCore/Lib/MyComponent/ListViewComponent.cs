using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Lib;

// p.197 [Add] ビューコンポーネントを定義
// ※接尾辞「XxxxViewComponent」
public class ListViewComponent : ViewComponent // ViewComponentを継承
//【別解】
// [ViewComponent(Name="List")]
// public class MyClazz
{
    private readonly MyContext _db;

    // データベースコンテキストを準備（コンテキストの注入）
    public ListViewComponent(MyContext db)
    {
        this._db = db;
    }

    // ビューコンポーネントの実処理
    public async Task<IViewComponentResult> InvokeAsync(int price)
    {
        return View (
                    await _db.Books.Where(b => b.Price <= price).ToListAsync()
               );
    }
}
