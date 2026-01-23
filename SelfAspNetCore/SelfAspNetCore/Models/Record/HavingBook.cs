namespace SelfAspNetCore.Models.Record;

// p.283 [Add] Having句に相当する「GroupBy＋Where」メソッド（LINQメソッド構文）
public record HavingBook(string Publisher, int PriceAverage);
