using SelfAspNet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace SelfAspNet.Helpers;

public class MetaTagHelperComponent : TagHelperComponent
{
    private readonly MyContext _db;

    public MetaTagHelperComponent(MyContext db)
    {
        _db = db;
    }

    [ViewContext]
    public ViewContext? ViewContext { get; set; }

    public override async Task ProcessAsync(
      TagHelperContext context, TagHelperOutput output)
    {
        var controller = ViewContext?.RouteData.Values["controller"]?.ToString();
        var action = ViewContext?.RouteData.Values["action"]?.ToString();
        if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
        {
            var metas = await _db.Metas.Where(c =>
              c.Controller == controller && c.Action == action).ToListAsync();
            foreach (var meta in metas)
            {
                output.PostContent.AppendHtml(
                  $"<meta name=\"{meta.Name}\" content=\"{meta.Content}\" />");
            }
        }
    }
}