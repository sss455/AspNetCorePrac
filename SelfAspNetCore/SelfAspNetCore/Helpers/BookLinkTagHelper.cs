using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Helpers;

[HtmlTargetElement("book-link", Attributes="src")]
public class BookLinkTagHelper : TagHelper
{
    // src属性をBookプロパティに割り当て
    // この例であれば、<book-link>要素のsrc属性をBookプロパティに紐づけ。
    [HtmlAttributeName("src")]
    public Book? Book { get; set; }

    // attrs属性をAttributesプロパティに割り当て
    // この例であれば、<book-link>要素のattrs属性をAttributesプロパティに紐づけ。
    [HtmlAttributeName("attrs")]
    public object Attributes { get; set; } = new();


    // タグヘルパーの実処理（非同期版）
    public override async Task ProcessAsync(
        TagHelperContext context,  // タグに関わる情報(cf.167)
        TagHelperOutput output)    // タグ出力を生成するためのTagHelperOutput(cf.168)
    {
        //--------------------------------------------------------------------------
        //【出力結果イメージ】
        //   <figure>
        //     <a href="https://wings.msn.to/index.php/-/A-03/978-4-7981-8094-6/">
        //       <img alt="978-4-7981-8094-6"
        //            src="https://wings.msn.to/books/978-4-7981-8094-6/978-4-7981-8094-6.jpg"
        //            width="100">
        //     </a>
        //   </figure>
        //--------------------------------------------------------------------------

        // 書籍情報が空の場合は出力を破棄
        if(Book == null)
        {
            output.SuppressOutput();
            return;
        }

        // 本来の<book-link>要素をアンカータグに整形
        output.TagName = "a";
        output.Attributes.SetAttribute("href", $"https://wings.msn.to/index.php/-/A-03/{Book.Isbn}/");

        // 配下に埋め込む<img>要素を生成
        var builder = new TagBuilder("img");
        builder.MergeAttribute("src", $"https://wings.msn.to/books/{Book.Isbn}/{Book.Isbn}.jpg");
        builder.MergeAttribute("alt", Book.Isbn);
        builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(Attributes));

        // アンカータグの前後／配下にコンテンツを追加
        var content = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(
            $"<figcaption>{content.GetContent()}</figcaption>"
        );
        output.PreContent.SetHtmlContent(builder.RenderSelfClosingTag());
        output.PreElement.SetHtmlContent("<figure>");
        output.PostElement.SetHtmlContent("</figure>");
    }



}
