// p.388 値プロバイダー（QueryStringValueProviderFactoryのコードを読み解く）
#nullable enable

using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace Microsoft.AspNetCore.Mvc.ModelBinding;
namespace SelfAspNetCore.Lib.MyValueProvider;


// QueryStringValueProviderFactory
// ⇒値プロバイダー(QueryStringValueProvider)をインスタンス化するためのファクトリークラス
public class QueryStringValueProviderFactory 
                            : IValueProviderFactory
                        // ※ファクトリークラスの条件はIValueProviderFactoryインターフェイスを実装すること
{
    /// <inheritdoc />
    // 値プロバイダーを生成
    // ※IValueProviderFactoryインターフェイスで実装すべきは、本メソッドのみ。
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        // 引数contextがnullならば例外をスロー
        ArgumentNullException.ThrowIfNull(context);

        // クエリ情報のコレクションを取得
        IQueryCollection? queryCollection = context.ActionContext.HttpContext.Request.Query;
        if (queryCollection != null && queryCollection.Count > 0)
        {
            // 値プロバイダーの本体をインスタンス化
            var valueProvider = new QueryStringValueProvider(
                                                BindingSource.Query,           // データソース
                                                queryCollection,               // クエリ情報のコレクション
                                                CultureInfo.InvariantCulture); // カルチャ情報

            // あらかじめ用意されたプロバイダーリスト（ValueProviderFactoryContext#ValueProvidersプロパティ）に登録
            context.ValueProviders.Add(valueProvider);
        }

        return Task.CompletedTask;
    }
}

