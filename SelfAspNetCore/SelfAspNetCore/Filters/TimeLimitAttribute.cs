// p.407 [Add] フィルターパイプラインのスキップ
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNetCore.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TimeLimitAttribute : Attribute, IAuthorizationFilter // 承認フィルター
// AuthorizationFilterAttribute
{
    // キャンペーン期間の開始日時／終了日時プロパティ
    public DateTime Begin { get; init; }
    public DateTime End   { get; init; }

    // コンストラクター(Begin／Endプロパティを初期化)
    public TimeLimitAttribute(string begin, string end)
    {
        // 引数begin／endを解析（解析失敗の場合はDateTime.MinValue）
        DateTime.TryParse(begin, out var b);
        DateTime.TryParse(end,   out var e);

        // 開始日時＜終了日時でなければエラー
        if(b >= e) { throw new ArgumentException("開始日＜終了日で指定してください。"); }

        Begin = b;
        End = e;
    }

    /// <inheritdoc />
    // 承認フィルター
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 現在日時
        var now = DateTime.Now;

        // キャンペーン期間中（開始日時 ≦ 現在日時 ≦ 終了日時）でなければ、エラーメッセージを生成
        if( now < Begin || End < now)
        {
            // AuthorizationFilterContext#Resultプロパティを設定することで、後続の処理をスキップ
            context.Result = new ContentResult() {
                Content = $"このページは{Begin.ToShortDateString()}～{End.ToShortDateString()}の期間のみアクセスできます。"
            };
            // この例であれば、ContentsResultオブジェクトを設定しているので、期間外の場合はそのままエラーメッセージを表示して終了する
            // ※Controller派生クラスではないので、ヘルパーメソッドを利用できない。そのため、直接IActionResultオブジェクトを生成する
        }
    }
}
