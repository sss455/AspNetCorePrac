using SelfAspNet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SelfAspNet.Helpers;

[HtmlTargetElement("book-link", Attributes = "src")]
public class BookLinkTagHelper : TagHelper
{
  // 属性の準備
  [HtmlAttributeName("src")]
  public Book? Book { get; set; }

  [HtmlAttributeName("attrs")]
  public object Attributes { get; set; } = new();

  public override async Task ProcessAsync(
    TagHelperContext context, TagHelperOutput output)
  {
    if (Book == null)
    {
      output.SuppressOutput();
      return;
    }

    output.TagName = "a";
    output.Attributes.SetAttribute(
        "href", $"https://wings.msn.to/index.php/-/A-03/{Book.Isbn}/");

    var builder = new TagBuilder("img");
    builder.MergeAttribute("src",
        $"https://wings.msn.to/books/{Book.Isbn}/{Book.Isbn}.jpg");
    builder.MergeAttribute("alt", Book.Isbn);
    builder.MergeAttributes(
      HtmlHelper.AnonymousObjectToHtmlAttributes(Attributes));

    var content = await output.GetChildContentAsync();
    output.Content.SetHtmlContent(
      $"<figcaption>{content.GetContent()}</figcaption>");
    output.PreContent.SetHtmlContent(builder.RenderSelfClosingTag());
    output.PreElement.SetHtmlContent("<figure>");
    output.PostElement.SetHtmlContent("</figure>");
  }
}