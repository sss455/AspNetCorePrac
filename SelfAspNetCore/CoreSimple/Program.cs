// p.461 [Add] ミドルウェアの作成（Use／Runメソッド）
// Program.csだけで動作するアプリの例
// ※コントローラー／アクションに頼らず、ミドルウェアだけでレスポンスを生成する。
using CoreSimple.Lib.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// p.470 [Add] ミドルウェアクラスの作成
// 自作のミドルウェアクラスを登録
// app.UseMiddleware<HeadersInfoMiddleware>();
// 拡張メソッドを使用して登録
app.UseHeaderInfo();


//-----------------------------------------------------------------------------
// [構文] Useメソッド
//  public IApplicationBuilder Use( Func<HttpContext, Func<Task>, Task> middleware )
//    ※middleware: ミドルウェアの本体
//-----------------------------------------------------------------------------
// 1個目のミドルウェア
app.Use( async (context, next) =>
{
   var res = context.Response;
   res.ContentType = "text/html; charset=utf-8";
   await res.WriteAsync("<p>Use1: Before</p>");
   await next();
   await res.WriteAsync("<p>Use1: After</p>");
});

// 2個目のミドルウェア
app.Use( async (context, next) =>
{
   var res = context.Response;
   await res.WriteAsync("<p>Use2: Before</p>");
   await next();
   await res.WriteAsync("<p>Use2: After</p>");
});


// p.464 [Add] ミドルウェアの自作ーーMapメソッド
//-----------------------------------------------------------------------------
// [構文] Mapメソッド（指定のパスに合致した場合にのみ実行されるミドルウェアを追加）
//  public IApplicationBuilder Map(string pathMatch, Action<IApplicationBuilder> configuration)
//    ※pathMatch    : 実行判定に利用するパス
//    ※configuration: リクエストパスが引数pathMatchに前方一致した場合に実行する処理
//-----------------------------------------------------------------------------
//「～/current/...」で実行する処理
app.Map("/current", appCurrent =>
{
    /* 引数configuration(ラムダ式)の引数はIApplicationBuilderオブジェクトのため、
       一般的には、ラムダ式の中でさらにUse／Runメソッドを呼び出して、分岐先のパイプラインを決定する。*/

    // 終端ミドルウェア
    appCurrent.Run( async context =>
    {
        await context.Response.WriteAsync( $"<p>CurrentRun: {DateTime.Now.ToString()}</p>" );
    });
});

// p.464 [Add] ミドルウェアの自作ーーMapメソッド
//「～/random/...」で実行する処理
app.Map("/random", appRandom =>
{
    // 終端ミドルウェア
    appRandom.Run( async context =>
    {
        var r = new Random();
        await context.Response.WriteAsync( $"<p>RandomRun: {r.NextInt64(100)}</p>" );
    });
});


// p.466 [Add] ミドルウェアの自作ーーMapWhenメソッド
//-----------------------------------------------------------------------------
// [構文] MapWhenメソッド（指定の条件がtrueの場合にのみ実行されるミドルウェアを追加）
//  public IApplicationBuilder MapWhen(Func<HttpContext,bool> predicate, Action<IApplicationBuilder> configuration)
//    ※predicate    : 実行判定に利用する条件式
//    ※configuration: 引数predicateの条件式がtrueの場合に実行する処理
//-----------------------------------------------------------------------------
app.MapWhen(
    // 条件式
    context => context.Request.Query["isadmin"] == "true",
    // 条件式がtrueの場合に実行される「分岐先の処理」
    appPre =>
    {
        /* 引数configuration(ラムダ式)の引数はIApplicationBuilderオブジェクトのため、
           一般的には、ラムダ式の中でさらにUse／Runメソッドを呼び出して、分岐先のパイプラインを決定する。*/

        // 終端ミドルウェア
        appPre.Run(async context =>
        {
           await context.Response.WriteAsync("<p>AdminRun!!</p>");
        });
    }
);


//-----------------------------------------------------------------------------
// [構文] Runメソッド（Runメソッドによって登録されたミドルウェアを「終端ミドルウェア」または「ターミナルミドルウェア」と呼ぶ。）
//  public IApplicationBuilder Run(RequestDelegate middleware)
//    ※middleware: ミドルウェアの本体
//-----------------------------------------------------------------------------
// 終端ミドルウェア（それ以外のパスの場合）
app.Run(async context =>
{
    await context.Response.WriteAsync("Run!");
});


//-----------------------------------------------------------------------------
// [構文] Runメソッド（アプリ本体を起動する。ミドルウェアの登録とは別もの。）
//  public void Run(string? url  default)
//    ※default: 待ち受けするURL
//-----------------------------------------------------------------------------
// アプリを起動
app.Run();
// 引数urlで、下記のように呼び出しのアドレスを指定することも可能。
// ただし、一般的にはlaunchSettings.jsonで切り出すのが望ましい。
// app.Run("http://localhost:8080");


/* ↓ 実行結果の表示 ↓ /
  Use1: Before
  Use2: Before
  Run!
  Use2: After
  Use1: After
*/

/* ↓ 実行結果の表示「～/current」の場合 ↓ /
  Use1: Before
  Use2: Before
  CurrentRun: 2026/01/14 16:02:03
  Use2: After
  Use1: After
*/

/* ↓ 実行結果の表示「～/random」の場合 ↓ /
  Use1: Before
  Use2: Before
  RandomRun: 56
  Use2: After
  Use1: After
*/

/* ↓ 実行結果の表示「～/?isadmin==true」の場合 ↓ /
  Use1: Before
  Use2: Before
  AdminRun!!
  Use2: After 
  Use1: After
*/