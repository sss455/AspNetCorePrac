using System.ComponentModel;

namespace SelfAspNetCore.Models;

// p.279 [Add] レコードを使用し、Title／Priceプロパティだけを含んだオブジェクトを返す（GroupByメソッド）
public record MiniBook(
        [property: DisplayName("書名")] string Title,
        [property: DisplayName("価格")] int    Price
    );
