using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SelfAspNet.Helpers;

public static class StringHelpers
{
    // public static string Truncate(
    //   string text, int length = 15, string omission = "...")

    public static string Truncate(this IHtmlHelper helper,
        string text, int length = 15, string omission = "...")
    {
        if (text.Length <= length) { return text; }
        return text.Substring(0, length - 1) + omission;
    }

    public static IHtmlContent Cover(this IHtmlHelper helper,
    string isbn, object? htmlAttrs = null)
    {
        var builder = new TagBuilder("img");
        builder.MergeAttribute("src",
          $"https://wings.msn.to/books/{isbn}/{isbn}.jpg");
        builder.MergeAttribute("alt", isbn);
        builder.MergeAttributes(
          HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));
        return builder.RenderSelfClosingTag();
    }
}