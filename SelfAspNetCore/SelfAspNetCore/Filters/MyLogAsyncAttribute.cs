// p.403 [Add] フィルターの非同期化
using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNetCore.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false)]
public class MyLogAsyncAttribute : Attribute,
                                   IAsyncActionFilter // 
{
    // アクションの実行前に非同期に呼び出される
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        
        Console.WriteLine($"【Before】{context.ActionDescriptor.DisplayName}が実行されます。");

        // 引数のnext関数(デリゲート)で、アクション処理を明示的に呼び出す
        await next();
        // ※next関数を呼び出しがない場合、本来のアクションは実行されない。
        // ※アクションが実行されたあと、next関数以降の処理が実行される。
        
        Console.WriteLine($"【After】{context.ActionDescriptor.DisplayName}が実行されました。");
    }
}
