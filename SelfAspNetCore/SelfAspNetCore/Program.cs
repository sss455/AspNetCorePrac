using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Filters;
using SelfAspNetCore.Helpers;
using SelfAspNetCore.Lib;
using SelfAspNetCore.Lib.MyModelBinder;
using SelfAspNetCore.Lib.MyOptions;
using SelfAspNetCore.Lib.MyValueProvider;
using SelfAspNetCore.Models;

//===============================================================================================================================================
// builder
//===============================================================================================================================================
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.(訳)コンテナにサービスを追加する。
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    // p.390 [Mod] 自作の値プロバイダーをアプリに対して明示的に登録する
    options.ValueProviderFactories.Add(new HttpCookieValueProviderFactory());
    // ※AddControllersWithViewsメソッドのラムダ式はMvcOptionsオブジェクトを受け取るので、
    //   そのValueProviderFactories.Addメソッドでファクトリークラスを追加するだけ。

    // p.396 [Add] アプリに自作のバインダープロバイダーを登録する
    // options.ModelBinderProviders.Insert(0, new DateModelBinderProvider());

    // p.401 [Add] アプリ単位でフィルターの適用
    // options.Filters.Add<MyLogAttribute>();
    // ※この例の場合は、属性としての宣言ではないので、フィルタークラスにAttributeクラスを継承する必要はない。

    // p.405 [Add] フィルターの優先順位を設定する（アプリ単位）
    // options.Filters.Add<MyAppFilterAttribute>(int.MaxValue);

    // p.416 [Add] アプリグローバルにCSRF対策を準備するーーAutoValidateAntiforgeryToken属性
    // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    // ここではアプリ単位に適用しているが、属性なのでコントローラー単位に適用しても構わない。

    // p.421 [Add] 応答キャッシュを有効にするーーResponseCache属性の主なプロパティ(ChacheProfileName属性)
    // ※あらかじめキャッシュポリシーをアプリに登録しておく（"MyCache"はキャッシュのプロファイル名）
    options.CacheProfiles.Add("MyCache", new CacheProfile() {
        // CacheProfileクラスで利用できるプロパティはResponseCache属性のそれに準ずる
        Duration = 300 // 300秒
    });


});


// p.57 [Add] アプリにコンテキストを登録する
//                           <MyContext>:コンテキスト型
builder.Services.AddDbContext<MyContext>(options =>
    options
        .UseSqlServer( // SQL Server
            // 接続文字列
            builder.Configuration.GetConnectionString("MyContext")
         )
        // p.219 [Add] 遅延読み込み用のライブラリを追加（Microsoft.EntityFrameworkCore.Proxiesパッケージ）
        //.UseLazyLoadingProxies()
);


// p.144 [Add] ScriptTagHelperのイニシャライザーをサービスの一種としてアプリに登録
//  ※型引数：<ITagHelperInitializer<拡張するヘルパー型>, イニシャライザー型>
builder.Services.AddSingleton<ITagHelperInitializer<ScriptTagHelper>, ScriptTagHelperInitializer>();


// p.181 [Add] 自作のタグヘルパーコンポーネントをサービスとして登録
builder.Services.AddTransient<ITagHelperComponent, MetaTagHelperComponent>();


// p.364 [Add] iText7で動的にPDFデータを作成するーーFileXxxxxResultクラス(2) ※別解
builder.Services.Configure<KestrelServerOptions>(options =>
{
    // 応答への同期操作を明示的に許可する
    options.AllowSynchronousIO = true;
    // ※ASP.NET Coreではスレッドを効率的に利用するために、応答の同期操作を規定で禁止している。
    //   そのため、これを解除する必要がある。(設定を無効にした場合、例外が発生する)
});


// p.381 [Add] ファイルサイズの制約を解除する
// ※Kestrelでは、既定でリクエスト本体の最大サイズが約28.6MBに制限されているため、設定を追加。
//   Kestrel：ASP.NET Coreに標準で同梱されている組み込みサーバー
builder.WebHost.ConfigureKestrel(opts =>
{
    // ラムダ式の引数optsは、Kestrelの設定情報を表すKestrelServerOptionsオブジェクト。
    // そのLimits.MaxRequestBodySizeプロパティでリクエスト本文のサイズを設定。
    opts.Limits.MaxRequestBodySize = 1024 * 1024 * 55; // 55MB
});


// p.412 [Add] 依存性注入を伴うフィルター ーーServiceFilter属性
// p.413 [Add] 依存性を伴うフィルター    ーーIFilterFactoryインターフェイス
// フィルターをサービスとして登録
builder.Services.AddScoped<LogExceptionFilter>();


// p.420 [Add] 応答キャッシュを有効にするーーResponseCache属性の主なプロパティ(VaryByXxxxx属性)
// ResponseChachingミドルウェアを有効に設定
builder.Services.AddResponseCaching();


// p.475 [Add] 構成値を自作のクラスにマッピングするーーOptionsパターン
// アプリから利用できるように、対象のOptionsクラスに構成値をバインドする
builder.Services
    // 構成取得に利用するOptionsクラスを登録し、
    .AddOptions<MyAppOptions>()
    // 下記で指定した構成情報のサブセクションを、上記のOptionsクラスに流し込むイメージ
    .Bind( builder.Configuration.GetSection( nameof(MyAppOptions) ) );
    // ※サブセクションの取得
    //  「WebApplicationBuilderオブジェクト ＃ConfigrationManager   ＃GetSectionメソッド」
    //   (builder)                         ＃Configrationプロパティ ＃GetSectionメソッド

// // [別解] Configureメソッド
// builder.Services.Configure<MyAppOptions>( builder.Configuration.GetSection( nameof(MyAppOptions) ) );






//===============================================================================================================================================
// app
//===============================================================================================================================================
var app = builder.Build();

// // p.286 [Add] 複数のレコードをまとめて登録するAddRangeメソッド
// // ※サービス管理のためのIServiceProviderオブジェクトを取得する定型句
// using(var scope = app.Services.CreateScope())
// {
//  // var = IServiceProvider
//     var provider = scope.ServiceProvider;
//     // イニシャライザーで、Articlesテーブルに初期データを投入
//     await ArticleSeed.Initialize(provider);
// }
// ⇒影響しないようにコメントアウト


// Configure the HTTP request pipeline.                                                                                                               
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// p.432 [Add] ブラウザー未対応のHTTPメソッドをサポートする
// HttpMethodOverrideMiddlewareミドルウェアを有効に設定
// ※ブラウザーから送信されたHTTPメソッドを疑似的に別のメソッドに上書きをすることが可能になる
app.UseHttpMethodOverride( new HttpMethodOverrideOptions
{
    // ミドルウェアのオプション情報
    FormFieldName = "_method"  // 上書きに利用するフォーム要素(隠しフィールド)の名前
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// p.41 ルーティング設定
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// p.420 [Add] 応答キャッシュを有効にするーーResponseCache属性の主なプロパティ(VaryByXxxxx属性)
// ResponseChachingミドルウェアを有効に設定
app.UseResponseCaching();




app.Run();
