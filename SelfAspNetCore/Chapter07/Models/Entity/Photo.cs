using System;
using System.ComponentModel.DataAnnotations;

namespace Chapter07.Models;

// 画像情報テーブルエンティティ
public class Photo
{
    [Display(Name = "写真ID")]
    public int Id { get; set; }

    [Display(Name = "写真のファイル名")]
    public string Name { get; set; } = String.Empty;

    [Display(Name = "コンテンツタイプ")]
    public string ContentType { get; set; } = String.Empty;
    
    // ファイル本体を表すContentプロパティは、byte配列型として定義
    [Display(Name = "ファイル本体")]
    public byte[] Content { get; set; } = null!;
}
