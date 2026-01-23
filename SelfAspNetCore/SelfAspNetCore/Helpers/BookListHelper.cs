using System;
using System.Formats.Asn1;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Helpers;

// p.206 [Add] Razoreデリゲートをビューヘルパーに渡す
public static class BookListHelper
{
    public static IHtmlContent BookList(
        this IHtmlHelper helper,            　// 拡張メソッドとして追加する対象クラス
        IEnumerable<Book> books,              // Bookエンティティ配列
        Func<dynamic, IHtmlContent> template) // Razoreデリゲート
    {
        // <ul>要素を生成
        var builder = new TagBuilder("ul");

        // books配列を<li>リストに整形
        foreach(var book in books)
        {
            builder.InnerHtml
                .AppendHtml("<li>")
                .AppendHtml(template(book)) // 引数のRazoreデリゲートを呼び出し
                .AppendHtml("</li>");
        }
        
        // TagBuilderの内容をStringBuilder経由で出力
        using(var str = new StringWriter())
        {
            builder.WriteTo(str, HtmlEncoder.Default);
            return new HtmlString(str.ToString());    
        }
    }

}
