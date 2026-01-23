using System.ComponentModel;

namespace SelfAspNetCore.Models;

// p.279 [Add] レコードを使用し、複数列をキーにグループ化する（GroupByメソッド）
public record BookGroup(
        [property: DisplayName("出版社")] string Publisher,
        [property: DisplayName("刊行年")] int    Year
    );
