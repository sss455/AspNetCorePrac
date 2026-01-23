using System;
using Microsoft.EntityFrameworkCore;

namespace SelfAspNetCore.Models.Record.Seed;

// p.286 [Add] 複数のレコードをまとめて登録するAddRangeメソッド
public static class ArticleSeed
{
    // 初期データを投入するためのInitializeメソッド（イニシャライザー）
    public static async Task Initialize(IServiceProvider provider)
    {
        // 引数のIServiceProviderからデータベースコンテキストを自分でインスタンス化する場合の定型句
        // var = MyContext?
        using var db = new MyContext( provider.GetRequiredService<DbContextOptions<MyContext>>() );

        // Articlesテーブルにデータが存在するならば、処理を終了
        if( await db.Articles.AnyAsync() ) { return; }

        // Articlesテーブルにデータが存在しないならば、初期データを投入
        db.Articles.AddRange(
            // 1件目
            new Article
            {
                Title         = "ますます便利になるTypeScript！",
                Url           = "https://codezine.jp/article/corner/992",
                Category      = "JavaScript",
                CreatedAt     = new DateTime(2023, 12, 21),
                LastUpdatedAt = new DateTime(2023, 12, 22)
            },
            // 2件目
            new Article
            {
                Title         = "Remixを通じてWebを学ぶ",
                Url           = "https://codezine.jp/article/corner/942",
                Category      = "JavaScript",
                CreatedAt     = new DateTime(2023, 12, 23),
                LastUpdatedAt = new DateTime(2023, 12, 24)
            },
            // 3件目
            new Article
            {
                Title         = "Web Componentsを基礎から学ぶ",
                Url           = "https://codezine.jp/article/corner/927",
                Category      = "JavaScript",
                CreatedAt     = new DateTime(2023, 12, 25),
                LastUpdatedAt = new DateTime(2023, 12, 26)
            },
            // 4件目
            new Article
            {
                Title         = "Railsの新機能を知ろう！",
                Url           = "https://codezine.jp/article/corner/991",
                Category      = "Rails",
                CreatedAt     = new DateTime(2023, 12, 27),
                LastUpdatedAt = new DateTime(2023, 12, 28)
            },
            // 5件目
            new Article
            {
                Title         = "Railsによるクライアントサイド開発入門",
                Url           = "https://codezine.jp/article/corner/919",
                Category      = "Rails",
                CreatedAt     = new DateTime(2023, 12, 29),
                LastUpdatedAt = new DateTime(2023, 12, 30)
            },
            // 6件目
            new Article
            {
                Title         = "現場で役立つRust入門",
                Url           = "https://atmarkit.itmedia.co.jp/ait/series/36943/",
                Category      = "Rust",
                CreatedAt     = new DateTime(2023, 12, 31),
                LastUpdatedAt = new DateTime(2024, 1, 1)
            },
            // 7件目
            new Article
            {
                Title = "基本からしっかり学ぶRust入門",
                Url = "https://atmarkit.itmedia.co.jp/ait/series/24844/",
                Category = "Rust",
                CreatedAt = new DateTime(2024, 1, 2),
                LastUpdatedAt = new DateTime(2024, 1, 3)
            }
        );

        // データベースに反映
        await db.SaveChangesAsync();
    }
}
