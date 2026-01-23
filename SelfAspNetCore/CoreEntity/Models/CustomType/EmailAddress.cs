using System;

namespace CoreEntity.Models.CustomType;

// p.242 [Add] 特定の型全体に対してコンバーターを適用する（値コンバーター）
// EmailAddress型を準備し、メールアドレスをアプリで扱う場合にはEmailAddress型として、データベースに格納する際にはVARCHAR型として扱う。
public class EmailAddress
{
    // ローカル部、ドメイン部を表すプロパティ
    public string Local { get; init; }
    public string Domain { get; init; }

    // コンストラクター
    // 与えられたメールアドレスを分解してLocal／Domainプロパティに反映
    public EmailAddress(string mail)
    {
        var mails = mail.Split("@", 2);
        this.Local = mails[0];
        this.Domain = mails[1];
    }

    // 本来のメールアドレスの形式に変換
    public override string ToString()
    {
        return $"{Local}:{Domain}";
    }
}
