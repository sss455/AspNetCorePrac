// p.395 [Add] 自作のモデルバインダーをアプリ全体に適用する

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SelfAspNetCore.Lib.MyModelBinder;

public class DateModelBinderProvider
                    : IModelBinderProvider // モデルバインダーを作成するには、本インターフェイスを実装し、
                                           // そのGetBinderメソッドをオーバーライドするのが基本。
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if(context.Metadata.ModelType == typeof(DateTime))
        {
            return new DateModelBinder();
        }
        return null;
    }
}
