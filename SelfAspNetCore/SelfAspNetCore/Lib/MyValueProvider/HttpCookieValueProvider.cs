// p.389 [Add] 自作の値プロバイダーを実装する

using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace SelfAspNetCore.Lib.MyValueProvider;

// クッキーの値をバインドする自作の値プロバイダー★
public class HttpCookieValueProvider
                    : BindingSourceValueProvider, // 特定のソースから値を取得するためのプロバイダー
                      IEnumerableValueProvider    // 複数値を提供するためのプロバイダー
{
    // クッキー情報のコレクション★
    private readonly IRequestCookieCollection _cookieCollection;
    // キーを管理するためのコンテナー
    private PrefixContainer? _prefixContainer;


    // コンストラクター★
    public HttpCookieValueProvider(
                BindingSource bindingSource,
                IRequestCookieCollection cookieCollection, // クッキー情報のコレクション★
                CultureInfo? culture)                      // GetValueメソッドの戻り値であるValueProviderResultオブジェクトとともに返されるカルチャ情報
            : base(bindingSource)
    {
        // 引数がnullの場合に例外を発生
        ArgumentNullException.ThrowIfNull(bindingSource);
        ArgumentNullException.ThrowIfNull(cookieCollection);

        _cookieCollection = cookieCollection; // クッキー情報のコレクション★
        Culture = culture;                    // プロバイダーのためのカルチャ情報
    }


    // プロバイダーのためのカルチャ情報
    public CultureInfo? Culture { get; }


    // see PrefixContainer
    // キーの管理には、標準ライブラリに含まれるPrefixContainerクラスを利用。
    protected PrefixContainer PrefixContainer
    {
        get
        {
            // データソースを元に「キーを管理するためのコンテナー」を生成（すでにあれば、何もしない）
            if (_prefixContainer == null)
            {
                // PrefixContainerクラスは、管理すべきキー情報を渡すことでインスタンス化できる
                //（ここではクッキー情報コレクションのキー群を表す_values.Keysを渡す）★
                _prefixContainer = new PrefixContainer(_cookieCollection.Keys);
            }

            return _prefixContainer;
        }
    }


    /// <inheritdoc />
    // キーを管理するためのコンテナーに、引数prefixが含まれているかを判定
    //（指定したプレフィックスに合致するキーが存在するかを判定）
    public override bool ContainsPrefix(string prefix)
    {
        // PrefixContainer#ContainsPrefixメソッドを呼び出しているだけ
        return PrefixContainer.ContainsPrefix(prefix);
    }


    /// <inheritdoc />
    // キーを管理するためのコンテナーから引数prefixに合致するキー群を取得
    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
        // 引数prefixがnullの場合は例外をスロー
        ArgumentNullException.ThrowIfNull(prefix);

        // PrefixContainer#GetKeysFromPrefixメソッドを呼び出しているだけ
        return PrefixContainer.GetKeysFromPrefix(prefix);
    }


    /// <inheritdoc />
    // 引数cookieKeyに対応する値を取得
    public override ValueProviderResult GetValue(string cookieKey)
    {
        // 引数cookieKeyがnullの場合は例外をスロー
        ArgumentNullException.ThrowIfNull(cookieKey);

        // 引数cookieKeyが空文字列の場合は、結果値もなし
        if (cookieKey.Length == 0)
        {
            // 値が存在しない場合は、ValueProviderResult.Noneフィールドで空値を返す
            return ValueProviderResult.None;
        }

        // クッキー情報コレクションから、引数cookieKeyに合致する値を取得★
        string? cookieValues = _cookieCollection[cookieKey];
        if (string.IsNullOrEmpty(cookieValues)) // ★
        {
            // 合致する値が存在しない場合は、ValueProviderResult.Noneフィールドで空値を返す
            return ValueProviderResult.None;
        }
        else
        {
            // 合致する値があれば結果を返す
            // ※結果値は、ValueProviderResultオブジェクトとして返す
            return new ValueProviderResult(cookieValues, Culture);
        }
    }

}
