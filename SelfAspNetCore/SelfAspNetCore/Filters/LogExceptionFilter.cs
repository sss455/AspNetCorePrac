// p.409 [Add] 依存性を伴うフィルター ーーTypeFilter属性
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Filters;

public class LogExceptionFilter : IAsyncExceptionFilter // 承認フィルター 
                                                        // ※依存性注入を伴う場合には属性(Attribute)としては定義できない
{
    // データベースコンテキストを準備
    private readonly MyContext _db;

    // コンストラクタ
    public LogExceptionFilter(MyContext db)
    {
        // 依存性の注入
        _db = db;
    }

    /// <inheritdoc />
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        // DBコンテキストにErrorLogエンティティを追加
        _db.ErrorLogs.Add( new ErrorLog()
        {
            // ExceptionContextから例外情報を取得して設定
            Path       = context.HttpContext.Request.Path,   // リクエストパス
            Message    = context.Exception.Message,          // エラーメッセージ
            Stacktrace = context.Exception.StackTrace ?? "", // スタックトレース
            Accessed   = DateTime.Now                        // アクセス日時
        });

        // ErrorLogsテーブルに反映
        await _db.SaveChangesAsync();


        //--------------------------------------------------------------------
        // p.411 [Add] フィルター内部で例外処理を完結させる
        //  上記までの例では、アクションで発生した例外をASP.NET Coreにそのまま渡しているため、既定の例外ページが表示されている。
        //  例外フィルターで適切に処理されているのであれば、フィルター内部で完結させたい。
        //  これには、ExceptionContext＃ExceptionHandledプロパティをtrue設定に設定する。
        //  これで例外は処理済み(Handled)であることを、ASP.NET Coreに通知している。
        //--------------------------------------------------------------------
        // 例外が処理済みであることを宣言し、↓
        context.ExceptionHandled = true;
        // 例外ページ（Shared/MyError.cshtml）を表示する
        context.Result = new ViewResult()
        {
            ViewName = "MyError"
        };
    }
}
