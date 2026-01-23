using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEntity.Models;

// p.224 [Add] アノテーションによる規約のカスタマイズ
// 書籍情報エンティティ

// p.225 [Add] マッピング先のテーブルを指定する ーーー Table属性
// ・テーブル名を「Contents」に指定
[Table("Contents")]
public class Book
{
    public int Id { get; set; }

    // p.225 [Add] マッピング先の列を指定する ーーー Column属性
    // ・Isbn列の順序を1番目、CHCAR(17)型に指定。
    [Column(Order=0, TypeName="CHAR(17)")]
    public String Isbn { get; set; } = String.Empty;

    public String Title { get; set; } = String.Empty;

    // p.225 [Add] マッピング先の列を指定する ーーー Column属性
    // ・Price列⇒Amount列とし、順序を2番目、NVARCHCAR(50)型に指定。
    [Column("Amount", Order=1, TypeName="NVARCHAR(50)")]
    public int Price { get; set; }

    public String Publisher { get; set; } = String.Empty;

    public DateTime Published { get; set; }

    public bool Sample { get; set; }

    // // ナビゲーションプロパティ（リレーションシップ）
    // public ICollection<Review> Reviews { get; } = new List<Review>();
    // // ナビゲーションプロパティ（リレーションシップ）
    // public ICollection<Author> Authors { get; } = new List<Author>();
}
