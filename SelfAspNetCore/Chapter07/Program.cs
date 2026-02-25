using Chapter07.Lib;
using Chapter07.Models;
using Chapter07.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;
//using WebMiddleware.Data;


//===============================================================================================================================================
// builder
//===============================================================================================================================================
// builder：WebApplicationBuilderオブジェクト
// var builder = WebApplication.CreateBuilder(args);
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


// Add services to the container.(訳)コンテナにサービスを追加する。
builder.Services.AddControllersWithViews();


// p.57 [Add] アプリにコンテキストを登録する
builder.Services.AddDbContext<MyContext>(options => 
    options
        .UseSqlServer( // SQL Server
            // 接続文字列
            builder.Configuration.GetConnectionString("MyContext")
        )
);

//========================================================================================
// p.447 [Add] 自作サービスの登録
//========================================================================================
// [3] リポジトリクラスをアプリに登録する
// ※リポジトリクラスもサービスの一種なので、Program.csでアプリ(依存性注入システム)に登録しておく。
builder.Services.AddTransient<IBookRepository, BookRepository>();
// ⇒サービス群(IServiceCollection)には、WebApplicationBuilderオブジェクト(builder)のServicesプロパティからアクセスでき、
//   そのAddTransientメソッドで、自作のサービスを登録する。

// p.477 [Add] AddXxxxx拡張メソッド
// ※AddXxxxx拡張メソッドを準備しておくことで、下記のようなコードで、サービスの登録が可能。
// builder.Services.AddBookRepository();


// p.450 [Add] サービスのスコープ（有効期間）
// それぞれのスコープでサービスを登録
builder.Services.AddSingleton<ISingletonService, SingletonService>(); // AddSingleton：初回要求時に生成されたインスタンスを、アプリ起動中は維持。
builder.Services.AddScoped<   IScopedService,    ScopedService>();    // AddScoped   ：リクエスト単位で1つ、インスタンスを生成。
builder.Services.AddTransient<ITransientService, TransientService>(); // AddTransient：注入の都度、インスタンスを生成。


// p.453 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
// +--------------------------------------------+----------+---------+
// |                 メソッド                   | 複数実装 |   引数  |
// +--------------------------------------------+----------+---------+
// |  AddSingleton <IService, Impl>()           |    〇    |    ✕   |
// |     AddScoped <IService, Impl>()           |          |         |
// |  AddTransient <IService, Impl>()           |          |         |
// +--------------------------------------------+----------+---------+
// |  AddSingleton <IService>(sp = new Impl())  |    〇    |    〇   |
// |     AddScoped <IService>(sp = new Impl())  |          |         |
// |  AddTransient <IService>(sp = new Impl())  |          |         |
// +--------------------------------------------+----------+---------+
// |  AddSingleton <Impl>()                     |    ✕    |    ✕   |
// |     AddScoped <Impl>()                     |          |         |
// |  AddTransient <Impl>()                     |          |         |
// +--------------------------------------------+----------+---------+
// ※IServiceはサービス型、Impleは実装型、spはIServiceProvider型を表す。

// HttpContextAccessorサービスを追加（HttpContextクラスにアクセスするためのサービス）
builder.Services.AddHttpContextAccessor();
// IHogesServiceサービスを追加
builder.Services.AddTransient<IHogeService>(sp =>
{
    // 登録済みのHttpContextAccessorサービスを取得
    var context = sp.GetRequiredService<IHttpContextAccessor>();
    // 引数ありのサービスを登録
    return new HogeService(context.HttpContext?.Request.Headers.UserAgent);
});

// IMessageService型に対して複数の実装を登録
builder.Services.AddSingleton<IMessageService, MorningMessageService>();
builder.Services.AddSingleton<IMessageService, NightMessageService>();


// //========================================================================================
// // p.455 [Add] 特殊なサービス登録メソッド
// //========================================================================================
// // p.456 [Add] スコープによらない汎用的な「Addメソッド」
// //-----------------------------------------------------------------------------
// // [構文] Addメソッド
// //  public IServiceCollection Add(ServiceDescriptor descriptor)
// //  public IServiceCollection Add(IEnumberable<ServiceDescriptor> descriptor)
// //    ※descriptor: サービスの詳細情報
// //-----------------------------------------------------------------------------
// // [構文] ServiceDescriptorコンストラクターの一般的な構文
// //  public ServiceDescriptor(Type serviceType, 
// //                           Func<IServiceProvider, object> factory, 
// //                           ServiceLifetime lifetime)
// //    ※serviceType: サービスの型 
// //    ※factory    : 実装を生成するためのファクトリー
// //    ※lifetime   : サービスのスコープ
// //-----------------------------------------------------------------------------
// // ※Addメソッド(正しくはServiceDescriptorの生成)は冗長ですが、複数の関連するサービスをまとめて登録するような状況で利用できる。
// builder.Services.Add( new List<ServiceDescriptor>
// {
//     new ( //⇐「new ServiceDescriptor(...)」の省略構文。Listの型引数からServieDescriptorであることは明らかなので型を省略可能。
//         serviceType: typeof(IMessageService),          // サービスの型
//         factory:     _ => new MorningMessageService(), // 実装を生成するためのファクトリー(※1)
//         lifetime:    ServiceLifetime.Singleton         // サービスのスコープ
//     ),
//     // (※1)
//     //   引数factory(ラムダ式)は、引数としてIServiceProviderオブジェクトを受け取る。
//     //   しかし、この例では単にインスタンスを生成するだけのファクトリーで、引数は利用しないので、_で変数を破棄する旨を宣言している。
//     //   またIServiceProviderは、登録済みのサービスを管理するためのオブジェクト。別のサービスに基づいて、新たなサービスを生成したい場合などに利用する。
//     new (
//         typeof(IMessageService),
//         _ => new NightMessageService(),
//         ServiceLifetime.Singleton
//     )
// });
// // 上記ではコンストラクター経由でServiceDescriptorをインスタンス化したが、
// // 下記のようにSingleton／Scoped／Transientメソッドを利用することで、より簡単にServiceDescriptorを生成することもできる。
// builder.Services.Add( new List<ServiceDescriptor> {
//     ServiceDescriptor.Singleton<IMessageService, MorningMessageService>(),
//     ServiceDescriptor.Singleton<IMessageService, NightMessageService>()
// });


// // p.457 [Add] サービスが未登録の場合にだけ登録を実施する「TryAddXxxxxメソッド」
// // ※Xxxxxは Singleton、Scoped、Transientのいずれか。対応する構文はAddXxxxxメソッドと同様。
// // この例では、IMessageServiceサービスの実装として、先にMorningMessageServiceが登録されているので、NightMessageServiceは登録されない。
// builder.Services.AddSingleton<IMessageService, MorningMessageService>();
// builder.Services.TryAddSingleton<IMessageService, NightMessageService>();


// // p.457 [Add] 同一の実装型が未登録の場合にだけサービスを登録する「TryAddEnumerableメソッド」
// //-----------------------------------------------------------------------------
// // [構文] TryAddEnumerableメソッド
// //  public void TryAddEnumerable(ServiceDescriptor descriptor)
// //  public void TryAddEnumerable(IEnumberable<ServiceDescriptor> descriptor)
// //    ※descriptor: サービスの詳細情報
// //-----------------------------------------------------------------------------
// // この例では、IMessageService型は登録済みですが、その実装型であるNightMessageService型は未登録なので、
// // MorningMessageService／NightMessageServiceの双方とも登録される。
// builder.Services.AddSingleton<IMessageService, MorningMessageService>();
// builder.Services.TryAddEnumerable( ServiceDescriptor.Singleton<IMessageService, NightMessageService>() );








//===============================================================================================================================================
// app
//===============================================================================================================================================
var app = builder.Build();

//========================================================================================
// HTTPリクエストパイプライン（ミドルウェアパイプライン）を設定する
//  ・ミドルウェアの推奨順序
//    https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/?view=aspnetcore-10.0#middleware-order
//========================================================================================
if (app.Environment.IsDevelopment())
{
    // app.UseMigrationsEndPoint();          //  (1) マイグレーションの実行
}
//if (!app.Environment.IsDevelopment())
else
{
    app.UseExceptionHandler("/Home/Error");  //  (2) エラーページの設定
#region 
    // HSTSのデフォルト値は30日間です。本番環境では変更することをお勧めします。詳細はhttps://aka.ms/aspnetcore-hstsを参照してください。
#endregion
    app.UseHsts();                           //  (3) Strict-Transport-Security応答ヘッダーの設定
}

app.UseHttpsRedirection();                   //  (4) HTTPSリダイレクトの設定
app.UseStaticFiles();                        //  (5) 静的ファイルを提供
// app.UseCookiePolicy();                    //  (6) クッキーポリシーの設定

app.UseRouting();                            //  (7) ルーティングの設定
// app.UseRateLimiter();                     //  (8) レート(呼び出し制限回数)の設定
// app.UseRequestLocalization();             //  (9) ローカライズの設定
// app.UseCors();                            // (10) CORS(クロスオリジン要求)の設定

//app.UseAuthentication();                   // (11) 認証の設定
app.UseAuthorization();                      // (12) 認可の設定
// app.UseSession();                         // (13) セッションの設定
// app.UseResponseCompression();             // (14) 応答の圧縮の設定
// app.UseResponseCaching();                 // (15) 応答キャッシュの設定

// app.MapRazorPages();
// app.MapDefaultControllerRoute();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
