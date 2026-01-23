using System;
using CoreEntity.Models.CustomType;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreEntity.Lib.CustomTypeConverter;

// p.243 [Add] 特定の型全体に対してコンバーターを適用する（値コンバーター）
// EmailAddress型を準備し、メールアドレスをアプリで扱う場合にはEmailAddress型として、データベースに格納する際にはVARCHAR型として扱う。
public class EmailAddressConverter : ValueConverter<EmailAddress, string>
{
    // コンストラクター
    public EmailAddressConverter()
        // 基底クラスのコンストラクターに、変換ルールを(ラムダ式)を引き渡す
        : base(
            v => v.ToString(),        // 書き込み時：EmailAddress#ToStoringメソッドで文字列化
            v => new EmailAddress(v)) // 読み込み時：EmailAddressオブジェクトをインスタンス化
    {
        ;
    }
}
