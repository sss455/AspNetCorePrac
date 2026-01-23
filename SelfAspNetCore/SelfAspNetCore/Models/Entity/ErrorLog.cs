using System;
using System.ComponentModel.DataAnnotations;

namespace SelfAspNetCore.Models;

// 例外情報テーブルエンティティ
public class ErrorLog
{
    [Display(Name = "エラーログID")]
    public int Id { get; set; }

    [Display(Name = "リクエストパス")]
    public string Path { get; set; } = String.Empty;

    [Display(Name = "エラーメッセージ")]
    public string Message { get; set; } = String.Empty;

    [Display(Name = "スタックトレース")]
    public string Stacktrace { get; set; } = String.Empty;

    [Display(Name = "アクセス日時")]
    public DateTime Accessed { get; set; }
}
