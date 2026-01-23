using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SelfAspNetCore.Lib.MyActionResult;

// p.364 [Add] IActionResult実装クラスの自作
public class CsvResult : ActionResult // 継承：ActionResultクラスはIActionResultインターフェイスの基本実装。
                                      //      IActionResultインターフェイスを直接実装しても構わないが、
                                      //      一般的にはActionResultクラスを継承するのが簡便。
{
    // CSV形式に変換する対象(モデル)を準備
    readonly IEnumerable<object> _list;

    // コンストラクター
    public CsvResult(IEnumerable<object> list)
    {
        _list = list;
    }

    // アクション結果の処理方法を定義(override)
    //  【Actionresultクラスの主なメソッド】
    //    ・ExecuteResult(context)     ：アクションの処理結果を同期的に実行
    //    ・ExecuteResultAsync(context)：アクションの処理結果を非同期に実行
    //   ※上記メソッドはいずれもアクションが終了したときにASP.NET Coreによって呼び出され、何らかの応答を生成する。
    //
    //  2個のExecuteResultXxxxxのうち、非同期版のExecuteResultAsyncメソッドは、既定でExecuteResult(同期版)を呼び出す。
    //  よって、最低限の実装としてはExecuteResultメソッドを実装すればよいということになる。
    public override void ExecuteResult(ActionContext context)
    {
        // 追加の文字エンコーディングを登録
        // ※日本語系の文字エンコーディングを利用する場合には明示的に登録する必要がある（イディオム）
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // HttpResponseオブジェクトを、引数のコンテキスト情報(ActionContext)からHttpContext.Responseプロパティ経由で取得
        HttpResponse res = context.HttpContext.Response;

        // ヘッダー情報を宣言
        // ※HttpResponse#Headersプロパティ経由でContentType／ContentDispositionヘッダーを設定
        res.Headers.ContentType = "text/csv; charset=sjis";
        res.Headers.ContentDisposition = "attachment; filename=\"result.csv\"";

        // 生成したカンマ区切りテキストを出力
        // ※HttpResponse#WriteAsyncメソッドでレスポンス本体を書き込み
        //   レスポンス本体を(カンマ区切りテキスト)を生成しているのは自作のCreateCSVメソッド
        res.WriteAsync( CreateCSV(_list), Encoding.GetEncoding("Shift-JIS") );
        // ※Encoding.GetEncodingメソッドを呼び出しただけではShift-JISは有効にできない（エラーとなる）
        //   .NET 8で標準サポートされているエンコーディングは、Unicode系、ASCII、ISO-8859-1に限定されているため。
        //   それ以外の文字エンコーディングを利用する場合には、Encoding.RegisterProviderメソッドで命じてkに登録する必要がある（イディオム）
    }

    // 与えられたリストからカンマ区切りのテキストを出力
    private static string CreateCSV(IEnumerable<object> list)
    {
        // カンマ区切りの文字列を保持するためのStringBuilder
        var sb = new StringBuilder();

        // 引数のオブジェクトのリストを順に走査
        foreach(var obj in list)
        {
            var rows = new List<string?>();

            // オブジェクトのすべてのプロパティを取得（Type#GetPropertiesメソッドでオブジェクトのすべてのプロパティを取得）
            // ※ActionResultとしては、どのようなモデルが渡されるかが事前に想定できないので、このようにTypeオブジェクト経由で型情報を取得している
            foreach(var prop in obj.GetType().GetProperties())
            {
                // 特定の型のみをリストに追加  ※ナビゲーションプロパティまでも取得してしまうため
                Type type = prop.PropertyType;
                //  [プリミティブ型]        [String型]              [DateTime型]
                if(type.IsPrimitive || type==typeof(string) || type==typeof(DateTime) )
                {
                    rows.Add(prop?.GetValue(obj)?.ToString());
                }
            }
            // リストの内容をカンマで連結、末尾に改行文字を加えたものを追加
            sb.AppendLine(string.Join(",", rows.ToArray()));
        }
        // 最終的に文字列化したものを返す
        return sb.ToString();
    }
}
