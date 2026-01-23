using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoreIdentity.Data;

//===============================================================================================================================================
// builder
//===============================================================================================================================================
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// (訳)コンテナにサービスを追加する。
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// p.424 [Auto]「ASP.NET Core Identity」プロジェクトの内容
// Identityサービスを有効化
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
                {
                    //自動生成時にこれだけ設定されていた
                    //options.SignIn.RequireConfirmedAccount = true;

                    // p.424 [Add] 配下のラムダ式は、Identityの諸機能に関するオプション設定
                    /* Userプロパティ */
                    //options.User.AllowedUserNameCharacters = ""; // ユーザー名に使用できる文字（既定は、半角英数字と記号 -._@+ ）
                    options.User.RequireUniqueEmail = false;       // 電子メールを必要とするか
                    /* SignInプロパティ */
                    options.SignIn.RequireConfirmedAccount   = true;    // ログインに確認済みのアカウントが必要か   （既定はfalse）
                    options.SignIn.RequireConfirmedEmail       = false; // ログインに確認済みのメールアドレスが必要か（既定はfalse）
                    options.SignIn.RequireConfirmedPhoneNumber = false; // ログインに確認済みの電話番号が必要か
                    /* Passwordプロパティ */
                    options.Password.RequireDigit           = true; // パスワードに数字を含める必要があるか  （既定はtrue）
                    options.Password.RequiredLength         = 6;    // パスワードの最小長                   （既定６）
                    options.Password.RequiredUniqueChars    = 1;    // パスワードに必須な一意の文字の最小数  （既定は１）
                    options.Password.RequireLowercase       = true; // パスワードに小文字のASCIIが必須化    （既定はtrue）
                    options.Password.RequireNonAlphanumeric = true; // パスワードに英数字以外の文字を含めるか（既定はtrue）
                    options.Password.RequireUppercase       = true; // パスワードに大文字のASCII文字が必須化（既定はtrue）
                    /* Lockoutプロパティ */
                    options.Lockout.AllowedForNewUsers      = true; // 新規ユーザーをロックアウトできるか（既定はtrue）
                    options.Lockout.DefaultLockoutTimeSpan  = new TimeSpan(5); // ロックされる時間     （既定は５）
                    options.Lockout.MaxFailedAccessAttempts = 5;    // ロックされるまでのアクセス試行回数（既定は５）
                })
                // p.427 [Add]現在のアプリでロール機能を有効に設定（イディオム）
                .AddRoles<IdentityRole>()
                // [Auto]
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();



//===============================================================================================================================================
// app
//===============================================================================================================================================
var app = builder.Build();

// Configure the HTTP request pipeline.
// (訳)HTTPリクエストのパイプラインを設定する。
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // (訳)HSTSのデフォルト値は30日間です。本番環境では変更することをお勧めします。詳細はhttps://aka.ms/aspnetcore-hstsを参照してください。
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// p.424 [Auto]「ASP.NET Core Identity」プロジェクトの内容
// Razor Pagesを有効化
// ※Identityで用意されたログインページなどがRazor Pagesで実装されているため。
app.MapRazorPages();


app.Run();
