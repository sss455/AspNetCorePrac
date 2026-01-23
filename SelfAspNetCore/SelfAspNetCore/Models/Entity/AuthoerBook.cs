using System;

namespace SelfAspNetCore.Models;

//「Booksテーブル⇔Authorsテーブル」の中間テーブル
// ※便宜的に「結合エンティティ」と呼ぶ。
public class AuthoerBook
{
    // ナビゲーションプロパティ（リレーションシップ）
    public ICollection<Author> Author { get; set; } = null!;

    // ナビゲーションプロパティ（リレーションシップ）
    public ICollection<Book> Book { get; set; } = null!;
}
