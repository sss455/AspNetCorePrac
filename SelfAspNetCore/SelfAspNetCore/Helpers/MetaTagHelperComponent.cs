using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Helpers;

public class MetaTagHelperComponent : TagHelperComponent
{
    private readonly MyContext _db;

    public MetaTagHelperComponent(MyContext db)
    {
        this._db = db;
    }

    [ViewContext]
    ViewContext? ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // 現在のコントローラー／アクションを取得
        var controller = ViewContext?.RouteData.Values["controller"]?.ToString();
        var action = ViewContext?.RouteData.Values["action"]?.ToString();

        // <head>要素が対象のときに編集作業を開始
        if(string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
        {
            // Metasテーブルから、対象ページのmeta情報を取得
            var metas = await _db.Metas.Where(m =>
                    m.Controller == controller && m.Action == action).ToListAsync();
            
            // 取得したデータを元に<meta>要素を生成
            foreach(var meta in metas)
            {
                output.PostContent.AppendHtml(
                    $"<meta name=\"{meta.Name}\" content=\"{meta.Content}\" />"
                );
            }
        }
    }
}
