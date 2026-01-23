// p.392 [Add] モデルバインダーの自作

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SelfAspNetCore.Lib.MyModelBinder;

public class DateModelBinder 
                    : IModelBinder // モデルバインダーを作成するには、本インターフェイスを実装し、
                                   // そのBindModelAsyncメソッドをオーバーライドするのが基本。
{
    // モデルに値を割り当てるためのメソッド
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        DateTime resultDateTime;

        try
        {
            resultDateTime = new DateTime(
                GetDateNumber(bindingContext, "year"),
                GetDateNumber(bindingContext, "month"),
                GetDateNumber(bindingContext, "day")
            );
        } catch {
            // 不正な日付値の場合はエラーを追加
            // ※ModelBindingContextからModelStateを取得し、エラー情報を設定
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "適切な日付値を入力してください。");
            return Task.CompletedTask;
        }

        // モデルに値を割り当て
        bindingContext.Result = ModelBindingResult.Success(resultDateTime);
        // ※モデルに値を割り当てるには、ModelBindingContext#Resultプロパティに、バインド値(ModelBindingResultオブジェクト)を渡すだけ。
        // ※ModelBindingResult.Successメソッドはバインドが成功したことを表すとともに、与えられた値をもとに、ModelBindingResultオブジェクトを生成する。
        // つまり、これがバインドのための最低限のコード。

        return Task.CompletedTask;
    }

    // 入力値（年／月／日）を取得
    private static int GetDateNumber(ModelBindingContext bindingContext, string type)
    {
        // 値プロバイダーから値を取得（ModelBindingContextオブジェクトのValueProvider.GetValueメソッド）
        ValueProviderResult value = bindingContext.ValueProvider.GetValue($"{bindingContext.ModelName}.{type}");
        // ※ここではフォーム要素の値が「モデル名.year」「モデル名.month」「モデル名.day」であることを前提としている。
        //   現在のモデル名は、ModelBindingContext#ModelNameプロパティで取得する。

        // 戻り値がない(ValueProviderResult.Noneである)場合には、既定値として0を返す
        if(value == ValueProviderResult.None) { return 0; }

        // 取得した値をInt32.TryParseメソッドで整数値に変換し、GetDateNumberメソッド全体の戻り値としている
        int.TryParse( (string?)value, out int result );

        // ※※※ ModelNameをキーとしたGetValueからの値取得から、TryParseによる目的の型への変換は定番の流れ ※※※

        return result;
    }
}
