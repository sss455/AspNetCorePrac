// p.390 [Add] 自作の値プロバイダーを実装する

using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SelfAspNetCore.Lib.MyValueProvider;

public class HttpCookieValueProviderFactory
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

        // クッキー情報のコレクションを取得★
        IRequestCookieCollection? cookieCollection = context.ActionContext.HttpContext.Request.Cookies;
        if (cookieCollection != null && cookieCollection.Count > 0)
        {
            // 値プロバイダーの本体をインスタンス化
            var valueProvider = new HttpCookieValueProvider(
                                                BindingSource.ModelBinding,    // データソース★
                                                cookieCollection,              // クッキー情報のコレクション
                                                CultureInfo.InvariantCulture); // カルチャ情報

            // あらかじめ用意されたプロバイダーリスト（ValueProviderFactoryContext#ValueProvidersプロパティ）に登録
            context.ValueProviders.Add(valueProvider);
            // ※自作の値プロバイダーの場合、アプリ(Program.cs)に対して明示的に登録する必要がある
        }

        return Task.CompletedTask;
    }
}
