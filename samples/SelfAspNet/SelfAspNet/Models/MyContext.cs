using Microsoft.EntityFrameworkCore;

namespace SelfAspNet.Models;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) { }
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;

    public DbSet<ErrorLog> ErrorLogs { get; set; } = default!;
    public DbSet<Article> Articles { get; set; } = default!;
    public DbSet<Meta> Metas { get; set; } = default!;

    public DbSet<Photo> Photos { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new TimestampInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Book>(e =>
        {
            e.HasData(
              new Book
              {
                  Id = 1,
                  Isbn = "978-4-7981-8094-6",
                  Title = "独習Java",
                  Price = 3960,
                  Publisher = "翔泳社",
                  Published = new DateTime(2024, 02, 15),
                  Sample = true
              },
              new Book
              {
                  Id = 2,
                  Isbn = "978-4-7981-7613-0",
                  Title = "Androidアプリ開発の教科書",
                  Price = 3135,
                  Publisher = "翔泳社",
                  Published = new DateTime(2023, 01, 24),
                  Sample = true
              },
              new Book
              {
                  Id = 3,
                  Isbn = "978-4-8156-1948-0",
                  Title = "これからはじめるReact実践入門",
                  Price = 4400,
                  Publisher = "SBクリエイティブ",
                  Published = new DateTime(2023, 09, 28),
                  Sample = true
              },
              new Book
              {
                  Id = 4,
                  Isbn = "978-4-296-07070-1",
                  Title = "作って学べるHTML＋JavaScriptの基本",
                  Price = 2420,
                  Publisher = "日経BP",
                  Published = new DateTime(2023, 07, 06),
                  Sample = false
              },
              new Book
              {
                  Id = 5,
                  Isbn = "978-4-297-13685-7",
                  Title = "Nuxt 3 フロントエンド開発の教科書",
                  Price = 3520,
                  Publisher = "技術評論社",
                  Published = new DateTime(2023, 09, 22),
                  Sample = false
              },
              new Book
              {
                  Id = 6,
                  Isbn = "978-4-297-13288-0",
                  Title = "JavaScript本格入門",
                  Price = 3520,
                  Publisher = "技術評論社",
                  Published = new DateTime(2023, 02, 13),
                  Sample = true
              },
              new Book
              {
                  Id = 7,
                  Isbn = "978-4-627-85711-7",
                  Title = "Pythonでできる! 株価データ分析",
                  Price = 2970,
                  Publisher = "森北出版",
                  Published = new DateTime(2023, 01, 21),
                  Sample = false
              },
              new Book
              {
                  Id = 8,
                  Isbn = "978-4-7981-7556-0",
                  Title = "独習C#",
                  Price = 4180,
                  Publisher = "翔泳社",
                  Published = new DateTime(2022, 07, 21),
                  Sample = true
              },
              new Book
              {
                  Id = 9,
                  Isbn = "978-4-8156-1336-5",
                  Title = "これからはじめるVue.js 3実践入門",
                  Price = 3740,
                  Publisher = "SBクリエイティブ",
                  Published = new DateTime(2022, 03, 19),
                  Sample = true
              },
              new Book
              {
                  Id = 10,
                  Isbn = "978-4-296-08014-4",
                  Title = "基礎からしっかり学ぶC#の教科書",
                  Price = 3190,
                  Publisher = "日経BP",
                  Published = new DateTime(2022, 03, 03),
                  Sample = false
              }
            );
        });

        builder.Entity<User>()
            .HasData(
                new User { Id = 1, Name = "山田祥寛", Email = "yyamada@example.com", Birth = new DateTime(1980, 01, 01) },
                new User { Id = 2, Name = "山内直", Email = "nyamauchi@example.com", Birth = new DateTime(1970, 04, 10) },
                new User { Id = 3, Name = "鈴木花子", Email = "hsuzuki@example.com", Birth = new DateTime(1990, 11, 24) },
                new User { Id = 4, Name = "佐藤太郎", Email = "tsato@example.com", Birth = new DateTime(1992, 08, 21) },
                new User { Id = 5, Name = "片渕彼富", Email = "kkatafuchi@example.com", Birth = new DateTime(1985, 10, 20) },
                new User { Id = 6, Name = "齊藤新三", Email = "ssaito@example.com", Birth = new DateTime(1974, 02, 15) },
                new User { Id = 7, Name = "鈴木花子", Email = "shanako@example.com", Birth = new DateTime(2000, 12, 01) },
                new User { Id = 8, Name = "高江賢", Email = "ktakae@example.com", Birth = new DateTime(1978, 06, 30) }
            );

        builder.Entity<Author>()
            .HasData(
                new Author { Id = 1, PenName = "WINGS", UserId = 1 },
                new Author { Id = 2, PenName = "Sarva", UserId = 6 },
                new Author { Id = 3, PenName = "たまデジ。", UserId = 2 },
                new Author { Id = 4, PenName = "Canon", UserId = 5 },
                new Author { Id = 5, PenName = "Papier", UserId = 8 },
                new Author { Id = 6, PenName = "はな", UserId = 3 }
            );

        builder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithMany(b => b.Authors)
            .UsingEntity<Dictionary<string, object>>(
                "AuthorBook",
                r => r.HasOne<Book>().WithMany().HasForeignKey("BooksId"),
                l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorsId"),
                je =>
                {
                    je.HasKey("BooksId", "AuthorsId");
                    je.HasData(
                        new { BooksId = 1, AuthorsId = 1 },
                        new { BooksId = 1, AuthorsId = 6 },
                        new { BooksId = 2, AuthorsId = 2 },
                        new { BooksId = 3, AuthorsId = 1 },
                        new { BooksId = 4, AuthorsId = 3 },
                        new { BooksId = 5, AuthorsId = 2 },
                        new { BooksId = 6, AuthorsId = 1 },
                        new { BooksId = 7, AuthorsId = 4 },
                        new { BooksId = 8, AuthorsId = 1 },
                        new { BooksId = 9, AuthorsId = 1 },
                        new { BooksId = 10, AuthorsId = 5 }
                    );
                }
            );

        builder.Entity<Review>()
            .HasData(
                new Review
                {
                    Id = 1,
                    Name = "山田太郎",
                    Body = "がっつり学習したい人には、うってつけの本です。前半は、キッチリ基礎固めを行い、後半は応用的な内容に。図や用例、構文も多く、辞書代わりにも使えます。",
                    LastUpdated = new DateTime(2024, 02, 20),
                    BookId = 1
                },
                new Review
                {
                    Id = 2,
                    Name = "佐藤花子",
                    Body = "環境構築から書かれており初心者から使えますが、分量から見てもわかるように、サラッとは終わらず、説明が深いです。特に「エキスパートに訊く」という読み物が面白い。",
                    LastUpdated = new DateTime(2024, 03, 01),
                    BookId = 1
                },
                new Review
                {
                    Id = 3,
                    Name = "鈴木次郎",
                    Body = "コツコツ、独りでじっくり勉強できます。オブジェクト指向プログラミングに紙数が割いてあって、以前、挫折したけど、今回は理解できました。1回目は読み飛ばしたところもあるので、また読み返したいです。",
                    LastUpdated = new DateTime(2024, 03, 10),
                    BookId = 1
                },
                new Review
                {
                    Id = 4,
                    Name = "井上裕子",
                    Body = "Androidはすぐにバージョンが上がるので、本を買うのもためらわれるが、この本は、基礎部分がしっかり説明されているので、はじめの一冊にお勧めできます。",
                    LastUpdated = new DateTime(2023, 11, 01),
                    BookId = 2
                },
                new Review
                {
                    Id = 5,
                    Name = "田中健太",
                    Body = "同様の構成で、Kotlin対応とJava対応があるので、両方手元にあると比較しながら勉強できます。",
                    LastUpdated = new DateTime(2023, 11, 30),
                    BookId = 2
                },
                new Review
                {
                    Id = 6,
                    Name = "藤井由美",
                    Body = "テスト、TypeScriptの活用、Next.jsの活用などなど。実践的な内容盛りだくさんで、かなりのボリュームです。サンプルも多く、ダウンロードして実際に動かしながら確認できて、理解が深まります。",
                    LastUpdated = new DateTime(2023, 12, 10),
                    BookId = 2
                }
            );
        builder.Entity<Article>(e =>
        {
            e.HasData(
                new Article
                {
                    Id = 1,
                    Title = "ますます便利になるTypeScript！",
                    Url = "https://codezine.jp/article/corner/992",
                    Category = "JavaScript",
                    CreatedAt = new DateTime(2023, 12, 21),
                    LastUpdatedAt = new DateTime(2023, 12, 22)
                },
                new Article
                {
                    Id = 2,
                    Title = "Remixを通じてWebを学ぶ",
                    Url = "https://codezine.jp/article/corner/942",
                    Category = "JavaScript",
                    CreatedAt = new DateTime(2023, 12, 23),
                    LastUpdatedAt = new DateTime(2023, 12, 24)
                },
                new Article
                {
                    Id = 3,
                    Title = "Web Componentsを基礎から学ぶ",
                    Url = "https://codezine.jp/article/corner/927",
                    Category = "JavaScript",
                    CreatedAt = new DateTime(2023, 12, 25),
                    LastUpdatedAt = new DateTime(2023, 12, 26)
                },
                new Article
                {
                    Id = 4,
                    Title = "Railsの新機能を知ろう！",
                    Url = "https://codezine.jp/article/corner/991",
                    Category = "Rails",
                    CreatedAt = new DateTime(2023, 12, 27),
                    LastUpdatedAt = new DateTime(2023, 12, 28)
                },
                new Article
                {
                    Id = 5,
                    Title = "Railsによるクライアントサイド開発入門",
                    Url = "https://codezine.jp/article/corner/919",
                    Category = "Rails",
                    CreatedAt = new DateTime(2023, 12, 29),
                    LastUpdatedAt = new DateTime(2023, 12, 30)
                },
                new Article
                {
                    Id = 6,
                    Title = "現場で役立つRust入門",
                    Url = "https://atmarkit.itmedia.co.jp/ait/series/36943/",
                    Category = "Rust",
                    CreatedAt = new DateTime(2023, 12, 31),
                    LastUpdatedAt = new DateTime(2024, 1, 1)
                },
                new Article
                {
                    Id = 7,
                    Title = "基本からしっかり学ぶRust入門",
                    Url = "https://atmarkit.itmedia.co.jp/ait/series/24844/",
                    Category = "Rust",
                    CreatedAt = new DateTime(2024, 1, 2),
                    LastUpdatedAt = new DateTime(2024, 1, 3)
                }
            );
            builder.Entity<Meta>(e =>
            {
                e.HasData(
                    new Meta
                    {
                        Id = 1,
                        Controller = "Home",
                        Action = "Privacy",
                        Name = "keywords",
                        Content = "メタ情報"
                    },
                    new Meta
                    {
                        Id = 2,
                        Controller = "Home",
                        Action = "Privacy",
                        Name = "description",
                        Content = "ページごとに異なる<meta>要素を生成"
                    },
                    new Meta
                    {
                        Id = 3,
                        Controller = "Tag",
                        Action = "Index",
                        Name = "description",
                        Content = "メタ情報取得の別解"
                    }
                );
            });
        });
    }
}