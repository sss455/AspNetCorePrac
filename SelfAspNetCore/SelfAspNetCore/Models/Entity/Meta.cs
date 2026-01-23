using System;
using System.ComponentModel.DataAnnotations;

namespace SelfAspNetCore.Models;

// メタ情報テーブルエンティティ
public class Meta
{
    [Display(Name = "メタID")]
    public int Id { get; set; }

    [Display(Name = "コントローラー名")]
    public string Controller { get; set; } = String.Empty;

    [Display(Name = "アクション名")]
    public string Action { get; set; } = String.Empty;

    [Display(Name = "<meta>要素のname属性")]
    public string Name { get; set; } = String.Empty;

    [Display(Name = "<meta>要素のcontent属性")]
    public string Content { get; set; } = String.Empty;
}
