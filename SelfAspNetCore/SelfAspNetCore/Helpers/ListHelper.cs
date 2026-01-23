using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace SelfAspNetCore.Helpers;

public static class ListHelper
{
    public static IHtmlContent RadioButtonListFor<TModel, TProperty> ( // TModel: モデルの型、TProperty：プロパティの型
        this IHtmlHelper<TModel>            helper,    // 拡張メソッドとして追加する対象クラス
        Expression<Func<TModel, TProperty>> exp,       // ラジオボタンの値を表すプロパティ（識別するための式）
        IEnumerable<SelectListItem>         itemList,  // ラジオボタンの選択オプション
        object?                             htmlAttrs) // ラベルに付与する任意のHTML属性
    {
        // ラジオボタンリストを束ねるためのStringBuilder
        var span = new StringBuilder();

        // ラムダ式からプロパティの名前／値を取得
        var provider = helper.ViewContext.HttpContext.RequestServices
                        .GetService(typeof(IModelExpressionProvider)) as ModelExpressionProvider;
        var meta = provider?.CreateModelExpression(helper.ViewData, exp);
        var name = provider?.GetExpressionText(exp);    // プロパティ名name(="Pulisher")
        var value = (string) meta?.Model!;              // プロパティ名value

        // 引数のSelectListItemリストからラジオボタンを生成
        var i = 1;
        foreach(var item in itemList)
        {
            // <label>要素を生成
            var label = new TagBuilder("label");
            label.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));

            // <label>配下に「ラジオボタン＋テキスト」を追加
            label.InnerHtml // InnerHtml：<label>要素は以下のコンテンツ(IHtmlContentBuilderオブジェクト)を取得
                .AppendHtml(
                    helper.RadioButton(
                                expression    : name,                        // 紐づけるプロパティ名
                                value         : item.Value,                  // ラジオボタンの値
                                isChecked     : item.Value == value,         // 選択状態にするか
                                htmlAttributes: new { id = $"{name}_{i++}" } // ラジオボタンに付与する任意の属性
                            )
                )
                .AppendHtml(item.Text);
                // <label cass="{htmlAttrs}">
                //     <input type="radio" name="publisher" value="{item.Value}" id="Publisher_{i++}" checked="{item.Value == value}">
                //      {item.Text}
                // </label>

            // 単一の<label>要素をStringBuilderに追加
            using(var writer = new StringWriter())
            {
                label.WriteTo(writer, HtmlEncoder.Default);
                span.Append(writer.ToString());
            }
        }
        // StringBuilder型をHtmlString形に変換
        return new HtmlString(span.ToString());


    }
}
