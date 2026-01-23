using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.Options;
using Microsoft.IdentityModel.Tokens;

namespace SelfAspNetCore.Lib.MyValidationAttribute;

// p.324 [Add] 検証属性の自作（サーバーサイドの実装）
// ※本格的な検証ルールを自作するならば、CusutomValidation属性よりも、検証属性そのものを自作するのが便利

// 検証属性の本体を定義するクラス
// ※入力値が、指定された候補値リストに含まれるかを判定するInOptions検証

// メタ属性
//  [AttributeUsage]         ：属性の仕様を明確にするためのメタ属性
//  　AttributeTargets.Property：プロパティに対して属性を付与する、という意味
//  　AllowMultiple=false      ：複数指定は許可しない、という意味
[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]

// 検証属性は、ValidationAttributeの派生クラスとして定義し、「＜検証名＞Attribute」の形式で命名するのが基本
//public class InOptionsAttribute : ValidationAttribute

// p.327 [mod] 検証属性の自作（クライアントサイドの実装）
// ※クライアントサイド検証を有効にするには、まず検証属性クラスに対して、検証に必要なパラメーター(（)検証名、エラーメッセージ)などを、
//   クライアントサイドに伝える必要がある。これには、IClientModelValidatorインターフェイスを継承し、そのAddValidationメソッドを実装する。
public class InOptionsAttribute : ValidationAttribute, IClientModelValidator
{
    // 候補値リストのプライベート変数
    private string _options;


    /// <summary>
    /// コンストラクター（候補値とエラーメッセージを設定）
    /// </summary>
    /// <param name="options">検証パラメーター（ここでは候補値のリスト）</param>
    public InOptionsAttribute(string options)
    {
        // 引数の候補値リストをプライベート変数に保持
        _options = options;
        // エラーメッセージを設定 「{0}=プロパティの表示名、{1}=候補値リスト」
        ErrorMessage = "{0} は 「{1}」 のいずれかの値で指定します。";
    }


    /// <summary>
    ///  プロパティの表示名と候補地リストでエラーメッセージを整形（＝プレイスホルダーを解決）して戻り値として返す<br/><br/>
    /// 
    ///  ※FormatErrorMessageメソッドの既定では、{0}だけを書しした結果を返すので、<br/>
    ///    {1}以降のプレイスホルダーを処理するならば、自らオーバーライドする必要がある。
    /// </summary>
    /// <param name="name">プロパティの表示名</param>
    /// <returns>整形後のエラーメッセージ</returns>
    public override string FormatErrorMessage(string name)
    {
        //【参考】
        // dt.ToString(); 
        // 実質的には dt.ToString(CultureInfo.CurrentCulture) と同義

        // プロパティの表示名と候補地リストでエラーメッセージを整形（＝プレイスホルダーを解決）して返却
        return String.Format(
                        CultureInfo.CurrentCulture, // 現在の地域情報(ロケール)
                        ErrorMessageString,         // 下記の記載参照
                        name,                       // {0}：プロパティの表示名
                        _options                    // {1}：候補値リスト
                    );
        //------------------------------------------------------------------------------------------
        //【ErrorMessageStringについて】
        //  整形対象のエラーメッセージとして、コンストラクターで初期化したErrorMessageプロパティではなく、
        //  ErrorMessageStringを参照している点に注目です。
        //
        //  これはエラーメッセージがErrorMessageプロパティではなく、
        //    ・ErrorMessageResourceType：リソースファイルの型
        //    ・ErrorMessageResourceName：リソースファイルのキー名
        //  から指定される場合を考慮しなければならないからです。（リソースについては8.5節）
        //  
        //  ErrorMessageStringプロパティは、ErrorMessageプロパティも含めたこれらのプロパティを適切に判断して、
        //  適切なエラーメッセージ（整形前）を返してくれるわけです。
        //  ErrorMessageプロパティを直接に参照してしまうとリソースファイルを利用している場合に、
        //  正しくメッセージが表示されないので要注意です。
        //------------------------------------------------------------------------------------------
    }


    /// <summary>検証の実処理（オーバーライド）</summary>
    /// <param name="value">検証対象の値（入力値）</param>
    /// <returns>検証の成否をブール値として返す</returns>
    public override bool IsValid(object? value)
    {
        //【参考】
        // C#でオブジェクトを文字列に変換するには主に.ToString()メソッドを使いますが、
        // as stringは「文字列型にキャストできるか試す」安全な方法です。
        // ToString()はオブジェクトがnullだとNullReferenceExceptionを起こしますが、
        // as stringはnullならnullを返し、nullでないならstring型に変換します。
        // 暗黙的な変換や複合書式設定でToString()は頻繁に使われ、string型自体のToString()は単にstringを返します
        var v = value as string;

        // 入力値が空の場合は検証をスキップ
        if(string.IsNullOrEmpty(v)) { return true;}

        // カンマ区切りの候補地リストを分解し、入力値valueと比較
        // IEnumerable<T>#Anyメソッド：リストに1つでも条件に合致するものがあればtrueを返す
        if( _options.Split(",").Any(opt => opt.Trim() == v) )
        {
            return true;
        }

        return false;
    }

    
    // p.327 [mod] 検証属性の自作（クライアントサイドの実装）
    // ※クライアントサイド検証を有効にするには、まず検証属性クラスに対して、検証に必要なパラメーター(検証名、エラーメッセージ)などを、
    //   クライアントサイドに伝える必要がある。これには、IClientModelValidatorインターフェイスを継承し、そのAddValidationメソッドを実装する。
    //
    // ClientModelValidationContext：クライアント検証に関する情報（主なプロパティはp328を参照）
    public void AddValidation(ClientModelValidationContext context)
    {
        // Attributesメソッド：紐づいたHTMLタグの属性群を取得
        // var = IDictionary<string, string>?
        var attrs = context.Attributes;

        // TryAddメソッド：HTMLタグの属性群(Dictionary)に対して「属性名：値」の組を追加する。
        //                属性が存在する場合はTryAddは失敗し、戻り値としてfalseを返す（ここではfalseを返しても、そのまま無視している）
        attrs.TryAdd("data-val", "true");                   // data-val              ：クライアント検証を有効化
        attrs.TryAdd("data-val-inoptions"                   // data-val-inptions     ：InOptions検証のエラーメッセージ
                        , this.FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        attrs.TryAdd("data-val-inoptions-opts", _options);  // data-val-inptions-opts：InOptions検証のパラメーター(候補値リスト)

        // 上記コードで、<input>要素に対して検証に関わる独自データ属性(data-xxxxx)を付与しただけ。
        // クライアントサイド検証を動作させるには、jQueryの検証ライブラリをインポートしている部分ビュー(_ValidationScriptsPartial.cshtml)に追記が必要。
    }
}
