using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace SelfAspNetCore.Lib;

// p.144 [Add] ScriptTagHelperのイニシャライザー
public class ScriptTagHelperInitializer 
    : ITagHelperInitializer<ScriptTagHelper>  // イニシャライザーはITagHelperInitializer<TTagHelper>インターフェイスの実装クラスとして定義
{
    // 初期化の処理を実装
    public void Initialize(ScriptTagHelper helper, ViewContext context)
    {
        // asp-append-version属性を有効化
        helper.AppendVersion = true;
    }
}
