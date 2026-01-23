using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Primitives;

namespace SelfAspNetCore.Lib;

public class RefererSelectorAttribute 
                        // セレクター属性を自作する場合、下記抽象クラスを継承する
                        : ActionMethodSelectorAttribute
{
    // 空のRefererヘッダーからのアクセスを認めるか
    public bool AllowNull { get; init; }


    // コンストラクター（デフォルトは空のRefereヘッダーからのアクセスを認める）
    public RefererSelectorAttribute(bool allowNull = true)
    {
        AllowNull = allowNull;
    }


    // ActionMethodSelectorAttribute抽象クラスでオーバーライドしなければならないのはIsValidForRequestメソッドのみ。
    // 本メソッドは、現在のリクエストに対して、そのアクションが妥当であるか(＝選択されるべきか)を判定し、その結果をブール値で返す。
    // falseの場合、アクションメソッドはルーティング規則にマッチしても実行されない。
    public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
    {
        // 引数のルートコンテキストからHttpRequestオブジェクトを取得
        HttpRequest? request = routeContext.HttpContext.Request;
        // HttpRequestオブジェクトからリクエストのRefererヘッダーを取得
        StringValues referer = request.Headers.Referer;

        // Refererヘッダーが空の場合は、AllowNullの設定にしたばう
        if(referer.Count == 0) { return AllowNull; }

        // Refererヘッダーに、現在のホスト情報が含まれているか判定
        bool result = referer[0]!.Contains($"{request.Host.Value}/");

        // true : 含まれている場合はアクセスを許可
        // false: 含まれていない場合はアクセスを禁止
        return result;
    }
}
