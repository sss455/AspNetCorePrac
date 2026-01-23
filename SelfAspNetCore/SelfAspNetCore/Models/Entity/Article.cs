using System;
using System.ComponentModel.DataAnnotations;
using SelfAspNetCore.Models.Interceptor;

namespace SelfAspNetCore.Models;

// 記事情報テーブルエンティティ
public class Article : IRecordableTimestamp
{
    [Display(Name = "記事ID")]
    public int Id { get; set; }

    [Display(Name = "記事タイトル")]
    public string Title { get; set; } = String.Empty;

    [Display(Name = "記事URL")]
    public string Url { get; set; } = String.Empty;

    [Display(Name = "カテゴリ")]
    public string Category { get; set; } = String.Empty;

    [Display(Name = "最終更新日時")]
    public DateTime LastUpdatedAt { get; set; }


    // p.302 [Add] エンティティを操作した前後の処理を実装する（インタセプタ―）
    [Display(Name = "作成日時")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "更新日時")]
    public DateTime UpdatedAt { get; set; }
}
