// p.470 [Add] ミドルウェアクラスの作成
namespace CoreSimple.Lib.Middlewares;

// 自作のミドルウェアをIApplicationBuilderの拡張メソッドとして登録。
// これにより、Program.csで標準のミドルウェアと同様に「UseXxxxx」のようなメソッドで登録できる。
public static class HeadersInfoMiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderInfo(this IApplicationBuilder builder)
    {
        //-----------------------------------------------------------------------------
        // [構文] UseMiddlewareメソッド
        //  public IApplicationBuilder UseMiddleware<TMiddleware>(params object?[] args)
        //    ※TMiddleware: ミドルウェアクラスの型
        //    ※middleware : ミドルウェアのコンストラクターに渡す引数(群)
        //-----------------------------------------------------------------------------
        return builder.UseMiddleware<HeadersInfoMiddleware>();
    }
}
