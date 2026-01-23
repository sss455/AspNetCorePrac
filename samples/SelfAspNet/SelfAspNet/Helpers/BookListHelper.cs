using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelfAspNet.Models;

namespace SelfAspNet.Helpers;
public static class BookListHelper
{
    public static IHtmlContent BookList(this IHtmlHelper helper,
    IEnumerable<Book> books, Func<dynamic, IHtmlContent> template)
    {
        var builder = new TagBuilder("ul");
        foreach (var book in books)
        {
            builder.InnerHtml
              .AppendHtml("<li>")
              .AppendHtml(template(book))
              .AppendHtml("</li>");
        }
        using (var str = new StringWriter())
        {
            builder.WriteTo(str, HtmlEncoder.Default);
            return new HtmlString(str.ToString());
        }
    }
}