// p.398 [Add] フィルターの基本ーーMyLogフィルター
using Microsoft.AspNetCore.Mvc.Filters;

namespace SelfAspNetCore.Filters;

// AttributeUsage：属性の仕様を明確にするためのメタ属性
//   AttributeTargets：属性を付与する対象はクラスまたはメソッド
//   AllowMultiple   ：複数指定（false=許可しない）
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false)]
public class MyLogAttribute
                    : Attribute,    // 属性の基底クラス。フィルターを属性として適用する場合は継承する。
                      IActionFilter // フィルターを実行したいタイミングに応じたIXxxxxFileterを実装。
{

    // アクションの実行後（IActionResultオブジェクトの実行前）
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"【Before】{context.ActionDescriptor.DisplayName}が実行されます。");
        // ※context.ActionDescriptor.DisplayNameプロパティで、「アクションの完全修飾名」を取得
    }

    // アクションの実行前（モデルバインドのあと）
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine($"【After】{context.ActionDescriptor.DisplayName}が実行されました。");
    }
}


// // p.403 [Add] XxxxxFilterAttribute抽象クラスを継承した例（↑と同じ意味）
// //
// // ■「XxxxxFilterAttribute抽象クラス」を用いるメリット
// //   (1)不要なメソッドは実装しなくてよい(抽象クラスなので)。
// //   (2)Attributeクラスを明示的に継承しなくてよい。
// // ※特別な理由が無いのであれば、XxxxxFilterAttribute抽象クラスを継承することをお勧め
// public class MyLogAttribute
//                     : // Attribute,            // 明示的に継承しなくてよい
//                          ActionFilterAttribute // XxxxxFilterAttribute抽象クラスを継承
// {
//     // アクションの実行後
//     public override void OnActionExecuting(ActionExecutingContext context)
//     {
//         Console.WriteLine($"【Before】{context.ActionDescriptor.DisplayName}が実行されます。");
//     }
// 
//     // アクションの実行前
//     public override void OnActionExecuted(ActionExecutedContext context)
//     {
//         Console.WriteLine($"【After】{context.ActionDescriptor.DisplayName}が実行されました。");
//     }
// }