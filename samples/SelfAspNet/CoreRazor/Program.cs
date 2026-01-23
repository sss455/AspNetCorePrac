using SelfAspNet.Models;
using Microsoft.EntityFrameworkCore;
using CoreRazor.Lib;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options => 
  options.UseSqlServer(
    builder.Configuration.GetConnectionString("MyContext")
  )
);

builder.Services.AddTransient<MyLogFilter>();

builder.Services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        // options.Filters.AddService<MyLogFilter>();
    });

// builder.Services.AddRazorPages(options =>
// {
//     // options.RootDirectory = "/Razor";

//     options.Conventions.AddFolderApplicationModelConvention(
//         "/Books",
//         model => model.Filters.Add(new MyLogFilter(builder.Configuration))
//     );
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
