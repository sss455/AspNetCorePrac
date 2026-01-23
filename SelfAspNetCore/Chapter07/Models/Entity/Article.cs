using System.ComponentModel.DataAnnotations;

namespace Chapter07.Models;

// 記事情報テーブルエンティティ
public class Article
{
    [Display(Name = "記事ID")]
    public int Id { get; set; }

    [Display(Name = "記事タイトル")]
    public string Title { get; set; } = String.Empty;

    [Display(Name = "記事URL")]
    public string Url { get; set; } = String.Empty;

    [Display(Name = "カテゴリ")]
    public string Category { get; set; } = String.Empty;


    [Display(Name = "作成日時")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "最終更新日時")]
    public DateTime LastUpdatedAt { get; set; }
}
