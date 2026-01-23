// p.385 値プロバイダー（QueryStringValueProviderのコードを読み解く）
#nullable enable

using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

//namespace Microsoft.AspNetCore.Mvc.ModelBinding;
namespace SelfAspNetCore.Lib.MyValueProvider;


// QueryStringValueProvider：標準の値プロバイダー
// ⇒クエリ文字列の「値プロバイダー」の本体
public class QueryStringValueProvider 
                    : BindingSourceValueProvider, // 特定のソースから値を取得するためのプロバイダー
                      IEnumerableValueProvider    // 複数値を提供するためのプロバイダー
                   // ※値プロバイダーであることの条件はIValueProviderインターフェイスを実装することですが、
                   //   IValueProviderには、さらに目的に特化した実装クラス／サブインターフェイスが用意されているため、
                   //   一般的にはそれらを継承／実装すればよい。
{
    // クエリ情報のコレクション
    private readonly IQueryCollection _queryCollection;
    // キーを管理するためのコンテナー
    private PrefixContainer? _prefixContainer;


    // コンストラクター
    public QueryStringValueProvider(
                BindingSource bindingSource,
                IQueryCollection queryCollection, // クエリ情報のコレクション
                CultureInfo? culture)             // GetValueメソッドの戻り値であるValueProviderResultオブジェクトとともに返されるカルチャ情報
            : base(bindingSource)
    {
        // 引数がnullの場合に例外を発生
        ArgumentNullException.ThrowIfNull(bindingSource);
        ArgumentNullException.ThrowIfNull(queryCollection);

        _queryCollection = queryCollection; // クエリ情報のコレクション
        Culture = culture;                  // プロバイダーのためのカルチャ情報
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
                //（ここではクエリ情報コレクションのキー群を表す_values.Keysを渡す）
                _prefixContainer = new PrefixContainer(_queryCollection.Keys);
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
    // 引数queryKeyに対応する値を取得
    public override ValueProviderResult GetValue(string queryKey)
    {
        // 引数queryKeyがnullの場合は例外をスロー
        ArgumentNullException.ThrowIfNull(queryKey);

        // 引数queryKeyが空文字列の場合は、結果値もなし
        if (queryKey.Length == 0)
        {
            // 値が存在しない場合は、ValueProviderResult.Noneフィールドで空値を返す
            return ValueProviderResult.None;
        }

        // クエリ情報コレクションから、引数queryKeyに合致する値を取得
        StringValues queryValues = _queryCollection[queryKey];
        if (queryValues.Count == 0)
        {
            // 合致する値が存在しない場合は、ValueProviderResult.Noneフィールドで空値を返す
            return ValueProviderResult.None;
        }
        else
        {
            // 合致する値があれば結果を返す
            // ※結果値は、ValueProviderResultオブジェクトとして返す
            return new ValueProviderResult(queryValues, Culture);
        }
    }
}
