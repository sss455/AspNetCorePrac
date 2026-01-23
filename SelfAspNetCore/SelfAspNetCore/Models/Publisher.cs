using System;
using System.ComponentModel.DataAnnotations;

namespace SelfAspNetCore.Models;

// p.124 [Mod] 列挙型から選択ボックスを生成する
public enum Publisher
{
    [Display(Name ="翔泳社")]
    SE,

    [Display(Name ="技術評論社")]
    Sihyo,

    [Display(Name ="SBクリエイティブ")]
    SB,

    [Display(Name ="日経BP社")]
    Nikkei,

    [Display(Name ="秀和システム")]
    Shuwa,
}
