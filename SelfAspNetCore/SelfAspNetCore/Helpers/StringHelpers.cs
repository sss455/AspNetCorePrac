using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace SelfAspNetCore.Helpers;

// p.153 [Add] シンプルなビューヘルパー
public static class StringHelpers
{
    /// <summary>
    ///  p.154 [Add] 与えられた文字列を指定の文字数で切り捨てるヘルパー
    ///  p.156 [Add] StringHelpers.TruncateをHtmlオブジェクトの拡張メソッドとして追加
    /// </summary>
    /// <param name="helper">拡張メソッドとして追加する対象クラス</param>
    /// <param name="text">対象の文字列</param>
    /// <param name="length">切り捨ての桁数（規定は15）</param>
    /// <param name="omission">切り捨て後に付与する文字列（規定は「...」）</param>
    /// <returns>切り捨て後の文字列</returns>
    public static string Truncate(this IHtmlHelper helper, string text, int length=15 , string omission="...")
    {
        // 指定文字数以内であれば、元の文字列を返す
        if(text.Length <= length) { return text; }

        // さもなければ、切り捨てた結果を返す
        return text.Substring(0, length-1) + omission;
    }

    /// <summary>
    ///  引数のISBNコードから書籍のカバー画像(<img>要素)を生成するヘルパー
    /// </summary>
    /// <param name="helper">拡張メソッドとして追加する対象クラス</param>
    /// <param name="isbn">ISBNコード</param>
    /// <param name="htmlAttrs"><img>に渡す属性群（「属性名＝値, ... 」の形式）</param>
    /// <returns>生成したHTML文字列</returns>
    public static IHtmlContent Cover(this IHtmlHelper helper, string isbn, object? htmlAttrs=null)
    {
        // TagBuilderで、<img>要素を生成
        var builder = new TagBuilder("img");

        // src、alt属性を付与（単一）
        builder.MergeAttribute("src", $"https://wings.msn.to/books/{isbn}/{isbn}.jpg");
        builder.MergeAttribute("alt", isbn);

        // その他の属性を付与（複数）
        builder.MergeAttributes(
            // 匿名型をIDictionary型に変換するとともに、プロパティ名に含まれる「_」を「-」に変換するためのメソッド
            HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));

        // 文字列に変換したものを返す
        return builder.RenderSelfClosingTag();
    }
}
