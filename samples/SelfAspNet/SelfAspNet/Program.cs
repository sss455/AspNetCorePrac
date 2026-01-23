using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Console;
using SelfAspNet.Filters;
using SelfAspNet.Lib;
using SelfAspNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using SelfAspNet;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SelfAspNet.Helpers;
using SelfAspNet.CompiledModels;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Mvc.ModelBinding;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddTransient<ITagHelperComponent, MetaTagHelperComponent>();

// Add services to the container.
// builder.Services.AddControllersWithViews()

builder.Services.AddControllersWithViews(options =>
{
    options.ValueProviderFactories.Add(new HttpCookieValueProviderFactory());
    // options.ModelBinderProviders.Insert(0, new DateModelBinderProvider());
    // options.Filters.Add<MyLogAttribute>();
    // options.Filters.Add<MyAppFilterAttribute>(int.MaxValue);
    // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    // options.CacheProfiles.Add("MyCache", new CacheProfile {
    //     Duration = 300
    // });
})
// .AddSessionStateTempDataProvider()
.AddViewLocalization(
    LanguageViewLocationExpanderFormat.Suffix,
    // LanguageViewLocationExpanderFormat.SubFolder,
    options => options.ResourcesPath = "Resources"
)
  .AddDataAnnotationsLocalization(options =>
  {
      options.DataAnnotationLocalizerProvider =
        (type, factory) => factory.Create(typeof(SharedResource));
  });

builder.Services.AddScoped<LogExceptionFilter>();

builder.Services.AddResponseCaching();

builder.Services.AddDbContext<MyContext>(options =>
    options
        // .UseLazyLoadingProxies()
        .UseSqlServer(
            builder.Configuration.GetConnectionString("MyContext")
        )
        // .UseModel(MyContextModel.Instance)
        // .UseSqlServer(builder.Configuration.GetConnectionString("MyContext"))
);

builder.Services.AddTransient<IBookRepository, BookRepository>();
// builder.Services.AddBookRepository();

// builder.Services.AddSingleton
//     <ITagHelperInitializer<ScriptTagHelper>,
//         ScriptTagHelperInitializer>();

builder.Services.AddSingleton<IMyService, MyService>();
// builder.Services.AddScoped<IMyService, MyService>();
// builder.Services.AddTransient<IMyService, MyService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHogeService>(sp =>
{
    var context = sp.GetRequiredService<IHttpContextAccessor>();
    return new HogeService(context.HttpContext?.Request.Headers.UserAgent);
});

builder.Services.AddSingleton<IMessageService, MorningMessageService>();
builder.Services.AddSingleton<IMessageService, NightMessageService>();
// builder.Services.TryAddEnumerable(
//   ServiceDescriptor.Singleton<IMessageService, NightMessageService>());

// builder.Services.Add(new List<ServiceDescriptor> {
//   new (
//     typeof(IMessageService),
//     _ => new MorningMessageService(),
//     ServiceLifetime.Singleton
//   ),
//   new (
//     typeof(IMessageService),
//     _ => new MorningMessageService(),
//     ServiceLifetime.Singleton
//   )

//   // ServiceDescriptor.Singleton<IMessageService, MorningMessageService>(),
//   // ServiceDescriptor.Singleton<IMessageService, NightMessageService>()
// });

// builder.Services.AddSingleton<IMessageService, MorningMessageService>();
// builder.Services.TryAddSingleton<IMessageService, NightMessageService>();

builder.Services.AddDistributedMemoryCache();

// builder.Services.AddDistributedSqlServerCache(options =>
// {
//   options.ConnectionString =
//     builder.Configuration.GetConnectionString("MyContext");
//   options.SchemaName = "dbo";
//   options.TableName = "MyCache";
// });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services
    .AddOptions<MyAppOptions>()
    .Bind(builder.Configuration.GetSection(nameof(MyAppOptions)))
    .ValidateDataAnnotations();

// builder.Services.Configure<MyAppOptions>(
//     builder.Configuration.GetSection(nameof(MyAppOptions)));

builder.Services
    .AddOptions<ApiInfoOptions>(ApiInfoOptions.SlideShow)
    .Bind(builder.Configuration.GetSection(
        $"{nameof(ApiInfoOptions)}:{ApiInfoOptions.SlideShow}"));

builder.Services
    .AddOptions<ApiInfoOptions>(ApiInfoOptions.OpenWeather)
    .Bind(builder.Configuration.GetSection(
        $"{nameof(ApiInfoOptions)}:{ApiInfoOptions.OpenWeather}"));

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// builder.WebHost.ConfigureKestrel(opts =>
// {
//     opts.Limits.MaxRequestBodySize = 1024 * 1024 * 55;
// });

builder.Configuration
//   .AddXmlFile("appsettings.xml", optional: true, reloadOnChange: true)
//   .AddXmlFile("appsettings.Development.xml", optional: true, reloadOnChange: true);
  .AddInMemoryCollection(
    new Dictionary<string, string?>
    {
        ["Company"] = "WINGSプロジェクト",
        ["WINGS-DM:Accept"] = "Yes",
        ["WINGS-DM:SendTime"] = "11:50:00",
        ["Logging:LogLevel:Default"] = "Information"
    }
  );

// builder.Logging.ClearProviders();

// builder.Logging.AddSimpleConsole(option =>
// {
//     option.IncludeScopes = true;
//     option.TimestampFormat = "F";
//     option.ColorBehavior = LoggerColorBehavior.Enabled;
// });

// builder.Logging.AddFile(
//     Path.Combine(builder.Environment.ContentRootPath, "Logs"));

builder.Services.Configure<RouteOptions>(options =>
    options.ConstraintMap.Add("isbn", typeof(IsbnRouteConstraint)));


var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var provider = scope.ServiceProvider;
//     await Seed.Initialize(provider);
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// app.UseStatusCodePages(
//     "text/html;charset=utf8", "{0}：ページを正しく表示できません...");
// app.UseStatusCodePagesWithRedirects("/error/catch/{0}");
// app.UseStatusCodePagesWithReExecute("/error/catch/{0}");

app.UseHttpsRedirection();

app.UseHttpMethodOverride(new HttpMethodOverrideOptions
{
    FormFieldName = "_method"
});

app.UseStaticFiles();
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//     Path.Combine(builder.Environment.ContentRootPath, "MyStorage")),
//     RequestPath = "/storage",
// });

// var provider = new FileExtensionContentTypeProvider();
// provider.Mappings.Add(".ace", "application/octet-stream");
// provider.Mappings.Remove(".gif");

// app.UseStaticFiles(new StaticFileOptions
// {
//     OnPrepareResponse = staticContext =>
//     {
//         staticContext.Context.Response.Headers.Append(
//           "Cache-Control", $"public, max-age={60 * 60 * 24 * 3}");
//     }

//     // ContentTypeProvider = provider

//     // ServeUnknownFileTypes = true,
//     // DefaultContentType = "application/octet-stream"
// });

// app.UseCookiePolicy(new CookiePolicyOptions
// {
//     CheckConsentNeeded = _ => true,
//     MinimumSameSitePolicy = SameSiteMode.Lax,
//     Secure = CookieSecurePolicy.Always
// });

// var fserver = new FileServerOptions {
//   EnableDefaultFiles = true,
//   EnableDirectoryBrowsing = true
// };
// fserver.DefaultFilesOptions.DefaultFileNames =
//   new List<string> { "home.html", "home.htm" };

// app.UseFileServer(fserver);

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.UseResponseCaching();

app.UseRequestLocalization(options =>
{
    var cultures = new[] { "ja", "de", "en" };
    // options
    //     .SetDefaultCulture(cultures[0])
    //     .AddSupportedCultures(cultures)
    //     .AddSupportedUICultures(cultures);
    //     .AddInitialRequestCultureProvider(new RouteValueRequestCultureProvider());

    // カルチャプロバイダーをProgram.csにハードコーディングする場合
    // .AddInitialRequestCultureProvider(new CustomRequestCultureProvider(
    //  async context =>
    //     {
    //         var routes = context.Request.RouteValues;
    //         if (routes == null) return await Task.FromResult(default(ProviderCultureResult));

    //         var culture = (string?)routes["culture"];
    //         if (culture == null) return await Task.FromResult(default(ProviderCultureResult));

    //         return await Task.FromResult<ProviderCultureResult?>(
    //         new ProviderCultureResult(culture));
    //     }
    // ));

    // var providers = options.RequestCultureProviders;
    // var query = providers[0];
    // providers.RemoveAt(0);
    // providers.Add(query);
});

app.Use(async (context, next) =>
{
    context.Items["current"] = DateTime.Now;
    await next.Invoke();
});

// ルーティング
// app.MapControllerRoute(
//     name: "article",
//     pattern: "article/{aid}",
//     // pattern: "article/{aid:int}",
//     // pattern: @"article/{aid:regex(^\d{{1,3}}$)}",
//     // pattern: @"article/{aid:length(17):regex(^978-4-[0-9]{{2,5}}-[0-9]{{2,5}}-[0-9X]$)}",
//     // pattern: "article/{aid:int=13}",
//     // constraints: new
//     // {
//     //     aid = @"\d{1,3}"
//     //     // aid =  new IntRouteConstraint()
//     // },
//     defaults: new
//     {
//         controller = "Route",
//         action = "Param"
//     }
// );

// app.MapControllerRoute(
//     name: "multiparam",
//     pattern: "article/{category}_{subcategory}/{aid}-{page}",
//     defaults: new
//     {
//         controller = "Route",
//         action = "ParamMulti"
//     }
// );

// app.MapControllerRoute(
//     name: "content",
//     pattern: "content/{code:isbn}",
//     // pattern: "content/{code:isbn(false)}",
//     defaults: new {
//         controller = "Route",
//         action = "Constraint"
//     }
// );

// app.MapControllerRoute(
//     name: "search",
//     pattern: "search/{**keywords}",
//     defaults: new
//     {
//         controller = "Route",
//         action = "Search"
//     }
// );

// app.MapControllerRoute(
//     name: "limit",
//     pattern: "campaign",
//     defaults: new {
//         controller = "Route",
//         action = "Limit"
//     },
//     constraints: new {
//         limit = new TimeLimitRouteConstraint("2024-01-01", "2024-06-30")
//     }
// );

// app.MapControllerRoute(
//     name: "area-default",
//     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "culture",
    pattern: "i/{culture=ja}/{controller=Home}/{action=Index}/{id?}");

app.Run();
