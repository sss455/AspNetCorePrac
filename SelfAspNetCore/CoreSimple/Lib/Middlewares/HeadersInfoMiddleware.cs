// p.469 [Add] ミドルウェアクラスの作成

// ※ミドルウェアクラスでは、以下のメンバーを実装している必要がある（ただし、何らかのインターフェイスを実装している必要はない）
//  ・RequestDelegate型を受け取るコンストラクター
//  ・処理本体を表すInvokeAsyncメソッド
using System.Text;
using Microsoft.Extensions.Primitives;

namespace CoreSimple.Lib.Middlewares;

// 要求ヘッダー／応答ヘッダーそれぞれをログに出力する
public class HeadersInfoMiddleware
{
    // RequestDelegate型は、ミドルウェアパイプラインを次に進める、いわゆるnext関数を表すもの。
    private readonly RequestDelegate _next;
    private readonly ILogger<HeadersInfoMiddleware> _logger;

    // コンストラクター（next関数、ロガーの準備）
    public HeadersInfoMiddleware(
                    RequestDelegate next,
                    ILogger<HeadersInfoMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // ミドルウェアの処理本体
    public async Task InvokeAsync(HttpContext context)
    {
        // ヘッダー情報を蓄積する容器を準備
        var str = new StringBuilder();

        // 要求ヘッダーを「名前: 値」形式で書き出し
        str.AppendLine("=== Request Headers Info ======================================================");
        foreach(KeyValuePair<string, StringValues> header in context.Request.Headers)
        {
            str.AppendLine($"{header.Key}: {header.Value}");
        }


        //-----------------------------------------------------
        // 次のミドルウェアへ移動
        //-----------------------------------------------------
        // InvokeAsyncメソッドの中でnext関数を呼び出す必要がある。
        // ただし、構文が若干異なり、引数としてHttpContext型を明示的に渡さなければならない。
        await _next(context);


        // 応答ヘッダーを「名前: 値」形式で書き出し
        str.AppendLine("=== Response Headers Info ======================================================");
        foreach(KeyValuePair<string, StringValues> header in context.Response.Headers)
        {
            str.AppendLine($"{header.Key}: {header.Value}");
        }

        // StringBuilderの内容をログに出力
        _logger.LogInformation(str.ToString());
    }
}
