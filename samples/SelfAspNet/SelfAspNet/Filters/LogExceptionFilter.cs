using SelfAspNet.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SelfAspNet.Filters;

public class LogExceptionFilter : IAsyncExceptionFilter
{
    private readonly MyContext _db;
    // private readonly string _header;
    public LogExceptionFilter(MyContext db)
    // public LogExceptionFilter(MyContext db, string header)
    {
        _db = db;
        // _header = header;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        _db.ErrorLogs.Add(new ErrorLog
        {
            Path = context.HttpContext.Request.Path,
            Message = context.Exception.Message,
            Stacktrace = context.Exception.StackTrace ?? "",
            Accessed = DateTime.Now
        });
        await _db.SaveChangesAsync();

        // context.ExceptionHandled = true;
        // context.Result = new ViewResult
        // {
        //     ViewName = "MyError"
        // };
    }
}