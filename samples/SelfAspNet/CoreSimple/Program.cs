using CoreSimple.Lib;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHeadersInfo();
app.Use(async (context, next) =>
{
    var res = context.Response;
    res.ContentType = "text/html;charset=utf-8";
    await res.WriteAsync("<p>Use1:Before</p>");
    await next();
    await res.WriteAsync("<p>Use1:After</p>");
});

app.Use(async (context, next) =>
{
    var res = context.Response;
    await res.WriteAsync("<p>Use2:Before</p>");
    await next();
    await res.WriteAsync("<p>Use2:After</p>");
});

app.Map("/current", appCur =>
{
    appCur.Run(async context =>
    {
        await context.Response.WriteAsync(
          $"<p>CurrentRun：{DateTime.Now.ToString()}</p>");
    });
});

app.Map("/random", appRan =>
{
    var r = new Random();
    appRan.Run(async context =>
    {
        await context.Response.WriteAsync($"<p>RandomRun：{r.NextInt64(100)}</p>");
    });
});

app.MapWhen(
  context => context.Request.Query["isadmin"] == "true",
// context => context.Request.Query.ContainsKey("isadmin"),
  appPre =>
  {
      appPre.Run(async context =>
      {
          await context.Response.WriteAsync($"<p>AdminRun!!</p>");
      });
  }
);

app.Run(async context =>
{
    await context.Response.WriteAsync("Run!!");
});

app.Run();