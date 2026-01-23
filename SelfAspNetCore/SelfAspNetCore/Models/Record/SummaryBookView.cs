using System.ComponentModel.DataAnnotations;

namespace SelfAspNetCore.Models;

// p.274 [Add] 特定のプロパティだけを取得するSelectメソッド（LINQメソッド構文）

// ビューとして表示／操作すべきデータを表す。
// このようなモデルのことをビューモデルと言う。
// これまではエンティティをそのままビューモデルとして利用してきたが、たまたま表示するべきデータとエンティティが管理する項目とが一致していたただけで、むしろ例外的な状況。
// 複数のエンティティを組み合わせることもあるし、エンティティとは無関係な項目をビューモデルとして表すこともある。モデルと一口に言っても、それが指すものは1つではない。
//
// ビューモデルであれば、あくまで値の受け渡しが目的で、プロパティ値をあとから修正することもないはずなので、レコードとして宣言するのがよりシンプル。
// （もちろん、従来のクラスでも構わない。）
public record SummaryBookView (
        // レコードではプロパティ名を引数として列挙しますが、そのままではDisplay属性を付与できないため、
        // [property:～]とプロパティへの修飾であることを明示している。
        [property: Display(Name="書名")]     string ShortTitle,
      //[property: Display(Name="値引価格")] int DiscountPrice,
        [property: Display(Name="値引価格"), DataType(DataType.Currency)] int DiscountPrice,
        [property: Display(Name="状態")]     string Released
    );