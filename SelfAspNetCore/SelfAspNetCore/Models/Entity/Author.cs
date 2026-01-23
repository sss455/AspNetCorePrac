using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfAspNetCore.Models;

// 著者情報テーブルエンティティ
public class Author
{
    // 著者ID
    public int Id { get; set; }

    [Display(Name = "著者名")]
    public string PenName { get; set; } = String.Empty;
    
    
    // 外部キー
    [Display(Name = "ユーザーID")]
    public int UserId { get; set; }

    // // ナビゲーションプロパティ（リレーションシップ）
    // [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    // // ナビゲーションプロパティ（リレーションシップ）
    public ICollection<Book> Books { get; } = new List<Book>();


    // // p.219 [Add]「ナビゲーションプロパティへのアクセス(遅延読み込み)」用にvirtual修飾子を付与。
    // // ナビゲーションプロパティ（リレーションシップ）
    // [ForeignKey(nameof(UserId))]
    // public virtual User User { get; set; } = null!;
    // public virtual ICollection<Book> Books { get; } = new List<Book>();
}
