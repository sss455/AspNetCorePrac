using System.Text;

namespace CoreSimple.Lib;

public class HeadersInfoMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<HeadersInfoMiddleware> _logger;

 public HeadersInfoMiddleware(RequestDelegate next,
    ILogger<HeadersInfoMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var str = new StringBuilder();
    str.AppendLine("===Request Headers Info===");
    foreach(var header in context.Request.Headers)
    {
      str.AppendLine($"{header.Key}: {header.Value}");
    }

    await _next(context);

    str.AppendLine("===Response Headers Info===");
    foreach(var header in context.Response.Headers)
    {
      str.AppendLine($"{header.Key}: {header.Value}");
    }
    _logger.LogInformation(str.ToString());
  }
}