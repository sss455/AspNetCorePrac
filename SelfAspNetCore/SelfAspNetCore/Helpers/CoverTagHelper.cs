using System;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Helpers;

// HtmlTargetElement属性で、タグヘルパーを適用する要素の構成を宣言
// この例であれば「<cover isbn="..." />」
//[HtmlTargetElement("cover", Attributes = "isbn", TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement(
        "cover",                                  // タグ名
        Attributes = "isbn",                      // 属性のリスト
        TagStructure = TagStructure.WithoutEndTag // 終了タグの省略を許容する
)]
public class CoverTagHelper : TagHelper // TagHelperクラスを継承
// タグヘルパーのクラス名は「ヘルパー名＋TagHelper」とする
{
    // isbn属性をIsbnプロパティに割り当て
    // この例であれば、<cober>要素のisbn属性をIsbnプロパティに紐づけ。
    [HtmlAttributeName("isbn")]
    public string Isbn { get; set; } = string.Empty;

    // タグヘルパーの実処理
    public override void Process(
        TagHelperContext context, // タグに関わる情報(cf.167)
        TagHelperOutput output)   // タグ出力を生成するためのTagHelperOutput(cf.168)
    {
        //-----------------------------------------------------------
        // まずは、引数output(TagHelperOutputクラス)を介して
        // 元々あったタグ(ここでは<cover>)を編集していく、と理解しておく。
        // この例では、タグ名を<img>に変更し、src／alt属性を追加している。
        //-----------------------------------------------------------
        // タグ名を修正
        output.TagName = "img";
        // src、alt属性を追加
        output.Attributes.SetAttribute("src",$"https://wings.msn.to/books/{Isbn}/{Isbn}.jpg");
        output.Attributes.SetAttribute("alt", Isbn);
    }
}
