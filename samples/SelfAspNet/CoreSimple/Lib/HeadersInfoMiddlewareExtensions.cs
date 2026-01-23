namespace CoreSimple.Lib;

public static class HeadersInfoMiddlewareExtensions
{
    public static IApplicationBuilder UseHeadersInfo(
      this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeadersInfoMiddleware>();
    }
}