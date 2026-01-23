using System;
using CoreEntity.Models.CustomType;
using CoreEntity.Models.MyEnum;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models;

//------------------------------------
// ユーザー情報テーブルエンティティ
//------------------------------------

// p.228 [Add] Index属性：特定の列にインデックスを設置する
// 「Nmae」プロパティでインデックスの名前を指定できる（規定は「IX_テーブル名_列名」）
[Index(nameof(LastName), nameof(FirstName), Name="Index_FullName")]

// インデックス列は、既定で昇順に並べられる。IsDescending
//「AllDescending」プロパティを false とすれば全てのインデックス列を降順とする
//[Index(nameof(LastName), nameof(FirstName), Name="Index_FullName", AllDescending=false)]

// 個々の列に昇順／降順を使い分けるならば、「IsDescending」を以下のように設定する。
// この例ではLastName列は昇順、FirstName列は降順にインデックスを生成する
[Index(nameof(LastName), nameof(FirstName), Name="Index_FullName", IsDescending = new[] { false, true})]

public class User
{
    public int Id { get; set; }

    public string LastName { get; set; } = String.Empty;

    public string FirstName { get; set; } = String.Empty;

    // p.242 [Mod] 特定の型全体に対してコンバーターを適用する（値コンバーター）
    // public string Email { get; set; } = String.Empty;
    public EmailAddress? Email { get; set; }

    public DateTime Birth { get; set; }

    // p.239 [Add] データベースの値を暗黙的に型変換する（値コンバーター）
    // UserClass列挙型のUserClassプロパティ
    public UserClass UserClass { get; set; } = UserClass.Guest;

    // // ナビゲーションプロパティ（リレーションシップ）
    // public Author? Author { get; set; }


}
